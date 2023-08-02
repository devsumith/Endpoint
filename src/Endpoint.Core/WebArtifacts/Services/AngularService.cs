// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Artifacts.Files.Factories;
using Endpoint.Core.Internals;
using Endpoint.Core.Messages;
using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Endpoint.Core.Syntax.Angular;
using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax.Properties;
using Endpoint.Core.Syntax.TypeScript;
using Endpoint.Core.Syntax;
using Endpoint.Core.WebArtifacts.Extenisons;


namespace Endpoint.Core.WebArtifacts.Services;

public class AngularService : IAngularService
{
    private readonly ILogger<AngularService> _logger;
    private readonly IArtifactGenerator _artifactGenerator;
    private readonly ICommandService _commandService;
    private readonly IFileProvider _fileProvider;
    private readonly IFileSystem _fileSystem;
    private readonly IFileModelFactory _fileModelFactory;
    private readonly Observable<INotification> _observableNotifications;
    private readonly IUtlitityService _utlitityService;
    private readonly ISyntaxService _syntaxService;
    private readonly INamingConventionConverter _namingConventionConverter;
    public AngularService(
        ILogger<AngularService> logger,
        IArtifactGenerator artifactGenerator,
        ICommandService commandService,
        IFileProvider fileProvider,
        IFileSystem fileSystem,
        IFileModelFactory fileModelFactory,
        Observable<INotification> observableNotifications,
        IUtlitityService utlitityService,
        ISyntaxService syntaxService,
        INamingConventionConverter namingConventionConverter
        )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
        _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _fileModelFactory = fileModelFactory ?? throw new ArgumentNullException(nameof(fileModelFactory));
        _observableNotifications = observableNotifications ?? throw new ArgumentNullException(nameof(observableNotifications));
        _utlitityService = utlitityService ?? throw new ArgumentNullException(nameof(utlitityService));
        _syntaxService = syntaxService ?? throw new ArgumentNullException(nameof(syntaxService));
        _namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter));
    }

    public void AddBuildConfiguration(string configurationName, AngularProjectReferenceModel model)
    {
        var fileReplacements = new List<FileReplacementModel>();

        var workspaceDirectory = Path.GetDirectoryName(_fileProvider.Get("angular.json", model.ReferencedDirectory));

        var jsonPath = $"{workspaceDirectory}{Path.DirectorySeparatorChar}angular.json";

        var json = JObject.Parse(_fileSystem.ReadAllText(jsonPath));

        json.AddBuildConfiguration(configurationName, model.Name, fileReplacements);

        _fileSystem.WriteAllText(jsonPath, JsonConvert.SerializeObject(json, Formatting.Indented));

        _commandService.Start($"ng generate environments --project {model.Name}", workspaceDirectory);

    }

    public void ComponentCreate(string name, string directory)
    {
        var nameSnakeCase = ((SyntaxToken)name).SnakeCase();

        var namePascalCase = ((SyntaxToken)name).PascalCase();

        _commandService.Start($"ng g c {name}", directory);

        var componentDirectory = $"{directory}{Path.DirectorySeparatorChar}{nameSnakeCase}";

        _artifactGenerator.CreateFor(_fileModelFactory.CreateTemplate(
            "Components.Default.Component",
            $"{nameSnakeCase}.component",
            componentDirectory,
            "ts",
            tokens: new TokensBuilder()
            .With("name", name)
            .Build()
            ));

        _artifactGenerator.CreateFor(_fileModelFactory.CreateTemplate(
            "Components.Default.Html",
            $"{nameSnakeCase}.component",
            componentDirectory,
            "html",
            tokens: new TokensBuilder()
            .With("name", name)
            .With("messageBinding", "{{ vm.message }}")
            .Build()
            ));

        var model = new FunctionModel() { Name = $"create{namePascalCase}ViewModel" };

        model.Imports.Add(new ImportModel("inject", "@angular/core"));

        model.Imports.Add(new ImportModel()
        {
            Module = "rxjs",
            Types = new() { new("map"), new("of") }
        });

        model.Body = new StringBuilder()
            .AppendLine($"return of(\"{name} works!\").pipe(")
            .AppendLine("map(message => ({ message }))".Indent(1, 2))
            .Append($");")
            .ToString();

        var fileModel = new ObjectFileModel<FunctionModel>(model, $"create-{nameSnakeCase}-view-model", componentDirectory, "ts");

        _artifactGenerator.CreateFor(fileModel);

        IndexCreate(false, componentDirectory);

        IndexCreate(false, directory);

        _observableNotifications.Broadcast(new FileCreated($"{componentDirectory}{Path.DirectorySeparatorChar}{nameSnakeCase}.component.ts"));

        _observableNotifications.Broadcast(new FileCreated($"{componentDirectory}{Path.DirectorySeparatorChar}{nameSnakeCase}.component.scss"));

        _observableNotifications.Broadcast(new FileCreated($"{componentDirectory}{Path.DirectorySeparatorChar}{nameSnakeCase}.component.html"));

        _observableNotifications.Broadcast(new FileCreated($"{componentDirectory}{Path.DirectorySeparatorChar}{nameSnakeCase}.component.spec.ts"));
    }

    public void ServiceCreate(string name, string directory)
    {
        _commandService.Start($"ng g s {name}", directory);

        var nameSnakeCase = ((SyntaxToken)name).SnakeCase();

        IndexCreate(false, directory);

        _observableNotifications.Broadcast(new FileCreated($"{directory}{Path.DirectorySeparatorChar}{nameSnakeCase}.service.ts"));

        _observableNotifications.Broadcast(new FileCreated($"{directory}{Path.DirectorySeparatorChar}{nameSnakeCase}.service.spec.ts"));
    }

    public void KarmaRemove(string directory)
    {
        var workspaceDirectory = Path.GetDirectoryName(_fileProvider.Get("angular.json", directory));

        _commandService.Start("npm uninstall @types/jasmine", workspaceDirectory);

        _commandService.Start("npm uninstall karma", workspaceDirectory);

        _commandService.Start("npm uninstall karma-chrome-launcher", workspaceDirectory);

        _commandService.Start("npm uninstall karma-coverage", workspaceDirectory);

        _commandService.Start("npm uninstall karma-jasmine", workspaceDirectory);

        _commandService.Start("npm uninstall karma-jasmine-html-reporter", workspaceDirectory);
    }

    public void JestInstall(string directory)
    {
        var workspaceDirectory = Path.GetDirectoryName(_fileProvider.Get("angular.json", directory));

        _commandService.Start("npm install -D jest@28.1.3 jest-preset-angular@12.2.6 @angular-builders/jest @types/jest --force", workspaceDirectory);
    }

    public void NgxTranslateAdd(string projectName, string directory)
    {
        var workspaceDirectory = _fileSystem.GetDirectoryName(_fileProvider.Get("angular.json", directory));

        _commandService.Start("npm install -D @ngx-translate/core --force", workspaceDirectory);

        _commandService.Start("npm install -D @ngx-translate/http-loader --force", workspaceDirectory);

        var model = new AngularProjectModel(projectName, "", "", directory);

        _addImports(model);

        _addHttpLoaderFactory(model);

        _registerTranslateModule(model);

        var i18nDirectory = $"{model.Directory}{Path.DirectorySeparatorChar}src{Path.DirectorySeparatorChar}assets{Path.DirectorySeparatorChar}i18n";

        _fileSystem.CreateDirectory(i18nDirectory);

        foreach (var lang in new string[] { "en", "fr" })
        {
            _fileSystem.WriteAllText($"{i18nDirectory}{Path.DirectorySeparatorChar}{lang}.json", "{}");
        }

        _setInitialLanguageInAppComponent(model);
    }

    public void CreateWorkspace(string name, string version, string projectName, string projectType, string prefix, string rootDirectory, bool openInVsCode = true)
    {
        var workspaceModel = new AngularWorkspaceModel(name, version, rootDirectory);

        _artifactGenerator.CreateFor(workspaceModel);

        _utlitityService.CopyrightAdd(workspaceModel.Directory);

        _commandService.Start("npm install npm-run-all --force", workspaceModel.Directory);

        KarmaRemove(workspaceModel.Directory);

        JestInstall(workspaceModel.Directory);

        var angularProjectModel = new AngularProjectModel(projectName, projectType, prefix, workspaceModel.Directory);

        AddProject(angularProjectModel);

        if (openInVsCode)
            _commandService.Start("code .", workspaceModel.Directory);

    }

    public void AddProject(AngularProjectModel model)
    {
        var stringBuilder = new StringBuilder().Append($"ng generate {model.ProjectType} {model.Name} --prefix {model.Prefix}");

        if (model.ProjectType == "application")
            stringBuilder.Append(" --style=scss --strict=false --routing");

        _commandService.Start(stringBuilder.ToString(), model.RootDirectory);

        var appRoutingModulePath = Path.Combine(model.Directory, "src", "app", "app-routing.module.ts");

        _fileSystem.Delete(appRoutingModulePath);

        var angularProjectReferenceModel = new AngularProjectReferenceModel(model.Name, model.Directory, model.ProjectType);

        EnableDefaultStandalone(angularProjectReferenceModel);

        ExportsAssetsAndStyles(angularProjectReferenceModel);

        if (model.ProjectType == "application")
        {
            var srcDirectory = $"{model.Directory}{Path.DirectorySeparatorChar}src{Path.DirectorySeparatorChar}";

            var appDirectory = $"{srcDirectory}app{Path.DirectorySeparatorChar}";


            var files = new List<FileModel>
            {
                _fileModelFactory.CreateTemplate("Angular.app.component","app.component", appDirectory, "ts"),
                _fileModelFactory.CreateTemplate("Angular.app.component.spec","app.component.spec", appDirectory, "ts"),
                _fileModelFactory.CreateTemplate("Angular.main","main", srcDirectory, "ts"),
            };

            foreach (var file in files)
            {
                _artifactGenerator.CreateFor(file);
            }

            _fileSystem.Delete($"{appDirectory}{Path.DirectorySeparatorChar}app.module.ts");

        }

        UpdateCompilerOptionsToUseJestTypes(model);

        JestConfigCreate(model);

        UpdateAngularJsonToUseJest(model);

        _utlitityService.CopyrightAdd(model.RootDirectory);




        if (model.ProjectType == "library")
        {
            //TODO: Move to JSON Extensions

            var packageJsonPath = _fileProvider.Get("package.json", model.RootDirectory);

            var packageJson = JObject.Parse(_fileSystem.ReadAllText(packageJsonPath));

            var watchLibs = packageJson["scripts"]["watch:libs"];

            var sanitizedName = model.Name.Replace("/", "-").Replace("@", string.Empty);

            packageJson["scripts"][$"watch:{_namingConventionConverter.Convert(NamingConvention.SnakeCase, sanitizedName)}"] = $"ng build {model.Name} --watch";

            if (string.IsNullOrEmpty($"{watchLibs}"))
            {
                packageJson["scripts"]["watch:libs"] = $"npm-run-all --parallel watch:{_namingConventionConverter.Convert(NamingConvention.SnakeCase, sanitizedName)}";
            }
            else
            {
                packageJson["scripts"]["watch:libs"] = $"{watchLibs} watch:{_namingConventionConverter.Convert(NamingConvention.SnakeCase, sanitizedName)}";
            }

            _fileSystem.WriteAllText(packageJsonPath, JsonConvert.SerializeObject(packageJson, Formatting.Indented));


            var publicApiPath = Path.Combine(model.Directory, "src", "public-api.ts");

            var publicApiContent = new List<string>();

            var libFolder = Path.Combine(model.Directory, "src", "lib");

            foreach (var file in Directory.GetFiles(libFolder, "*.*"))
            {
                _fileSystem.Delete(file);
            }

            _artifactGenerator.CreateFor(new ContentFileModel($"export const BASE_URL = '{_namingConventionConverter.Convert(NamingConvention.KebobCase, model.Name).ToUpper()}:BASE_URL';", "constants", libFolder, "ts"));

            IndexCreate(false, libFolder);

            foreach (var line in _fileSystem.ReadAllLines(publicApiPath))
            {

                if (!line.StartsWith("export"))
                {
                    publicApiContent.Add(line);
                }
            }

            publicApiContent.Add("export * from './lib';");

            _artifactGenerator.CreateFor(new ContentFileModel(new StringBuilder()
                .AppendJoin(Environment.NewLine, publicApiContent)
                .ToString(), "public-api", Path.Combine(model.Directory, "src"), "ts"));

        }
    }

    public void UpdateAngularJsonToUseJest(AngularProjectModel model)
    {
        var angularJsonPath = _fileProvider.Get("angular.json", model.Directory);

        var angularJson = JObject.Parse(_fileSystem.ReadAllText(angularJsonPath));

        var testJObject = new JObject
        {
            { "builder", "@angular-builders/jest:run" }
        };

        angularJson["projects"][model.Name]["architect"]["test"] = testJObject;

        _fileSystem.WriteAllText(angularJsonPath, JsonConvert.SerializeObject(angularJson, Formatting.Indented));
    }

    public void JestConfigCreate(AngularProjectModel model)
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.AppendLine("module.exports = {");

        stringBuilder.AppendLine("preset: 'jest-preset-angular',".Indent(1));

        stringBuilder.AppendLine("globalSetup: 'jest-preset-angular/global-setup'".Indent(1));

        stringBuilder.AppendLine("};");

        _fileSystem.WriteAllText($"{model.Directory}{Path.DirectorySeparatorChar}jest.config.js", stringBuilder.ToString());
    }

    public void ExportsAssetsAndStyles(AngularProjectReferenceModel model)
    {
        if (model.ProjectType == "application")
            return;

        var ngPackagePath = _fileProvider.Get("ng-package.json", model.ReferencedDirectory);

        var ngPackageJson = JObject.Parse(_fileSystem.ReadAllText(ngPackagePath));

        ngPackageJson.ExportsAssetsAndStyles();

        _fileSystem.WriteAllText(ngPackagePath, JsonConvert.SerializeObject(ngPackageJson, Formatting.Indented));
    }

    public void MaterialAdd(AngularProjectReferenceModel model)
    {
        var rootDirectory = Path.GetDirectoryName(_fileProvider.Get("angular.json", model.ReferencedDirectory));

        _commandService.Start($"ng add @angular/material --project {model.Name} --theme custom", rootDirectory);
    }

    public void EnableDefaultStandalone(AngularProjectReferenceModel model)
    {
        var angularJsonPath = _fileProvider.Get("angular.json", model.ReferencedDirectory);

        var angularJson = JObject.Parse(_fileSystem.ReadAllText(angularJsonPath));

        angularJson.EnableDefaultStandalone(model.Name);

        _fileSystem.WriteAllText(angularJsonPath, JsonConvert.SerializeObject(angularJson, Formatting.Indented));
    }

    public void UpdateCompilerOptionsToUseJestTypes(AngularProjectModel model)
    {
        var tsConfigSpecJsonPath = $"{model.Directory}{Path.DirectorySeparatorChar}tsconfig.spec.json";

        var tsConfigSpecJson = JObject.Parse(File.ReadAllText(tsConfigSpecJsonPath));

        tsConfigSpecJson.UpdateCompilerOptionsToUseJestTypes();

        _fileSystem.WriteAllText(tsConfigSpecJsonPath, JsonConvert.SerializeObject(tsConfigSpecJson, Formatting.Indented));
    }

    private readonly List<ImportModel> _importModels = new List<ImportModel>()
        {
            new ImportModel("HttpClient","@angular/common/http"),
            new ImportModel("TranslateLoader","@ngx-translate/core"),
            new ImportModel("TranslateModule ","@ngx-translate/core"),
            new ImportModel("TranslateHttpLoader ","@ngx-translate/http-loader")
        };
    private void _addImports(AngularProjectModel model)
    {
        var mainPath = $"{model.Directory}{Path.DirectorySeparatorChar}src{Path.DirectorySeparatorChar}main.ts";

        var lines = _fileSystem.ReadAllLines(mainPath);

        var content = string.Join(Environment.NewLine, lines);

        var newLines = new List<string>();

        var added = false;

        foreach (var line in lines)
        {
            if (!line.StartsWith("import") && !added)
            {
                foreach (var importModel in _importModels)
                {
                    if (!content.Contains(importModel.Types.First().Name))
                        newLines.Add(new StringBuilder()
                            .Append("import { ")
                            .Append(importModel.Types.First().Name)
                            .Append(" }")
                            .Append($" from '{importModel.Module}';")
                            .ToString());
                }

                added = true;
            }

            newLines.Add(line);

        }

        _fileSystem.WriteAllLines(mainPath, newLines.ToArray());
    }

    private void _addHttpLoaderFactory(AngularProjectModel model)
    {
        var mainPath = $"{model.Directory}{Path.DirectorySeparatorChar}src{Path.DirectorySeparatorChar}main.ts";

        var lines = _fileSystem.ReadAllLines(mainPath);

        var content = string.Join(Environment.NewLine, lines);

        var newLines = new List<string>();

        var added = false;

        foreach (var line in lines)
        {
            if (!line.StartsWith("import") && !added && !content.Contains("HttpLoaderFactory"))
            {
                newLines.Add("");

                foreach (var newLine in new string[3] {
                        "export function HttpLoaderFactory(httpClient: HttpClient) {",
                        "  return new TranslateHttpLoader(httpClient);",
                        "}"
                    })
                {
                    newLines.Add(newLine);
                }

                added = true;
            }

            newLines.Add(line);

        }

        _fileSystem.WriteAllLines(mainPath, newLines.ToArray());
    }

    private void _registerTranslateModule(AngularProjectModel model)
    {
        var mainPath = $"{model.Directory}{Path.DirectorySeparatorChar}src{Path.DirectorySeparatorChar}main.ts";

        var lines = _fileSystem.ReadAllLines(mainPath);

        var tabSize = 2;

        if (string.Join(Environment.NewLine, lines).Contains("TranslateModule.forRoot"))
            return;

        var newLines = new List<string>();

        var added = false;

        foreach (var line in lines)
        {
            if (line.Contains("importProvidersFrom(") && !added)
            {
                newLines.Add(line);

                foreach (var newLine in new string[]
                {
                    "HttpClientModule,",
                    "TranslateModule.forRoot({",
                    "loader: {".Indent(1,tabSize),
                    "provide: TranslateLoader,".Indent(2,tabSize),
                    "useFactory: HttpLoaderFactory,".Indent(2,tabSize),
                    "deps: [HttpClient]".Indent(2,tabSize),
                    "}".Indent(1,tabSize),
                    "}),"
                })
                {
                    newLines.Add(newLine.Indent(3, tabSize));
                }

                added = true;
            }
            else
            {
                newLines.Add(line);
            }
        }

        _fileSystem.WriteAllLines(mainPath, newLines.ToArray());
    }

    private void _setInitialLanguageInAppComponent(AngularProjectModel model)
    {
        var appComponentPath = $"{model.Directory}{Path.DirectorySeparatorChar}src{Path.DirectorySeparatorChar}app{Path.DirectorySeparatorChar}app.component.ts";

        var createConstructor = !_fileSystem.ReadAllText(appComponentPath).Contains("constructor");

        var tabSize = 2;

        var lines = _fileSystem.ReadAllLines(appComponentPath);

        var ctor = new string[]
        {
            "constructor(private readonly _translateService: TranslateService) {",
            $"_translateService.setDefaultLang(\"en\");".Indent(1,tabSize),
            $"_translateService.use(localStorage.getItem(\"currentLanguage\") || \"en\");".Indent(1,tabSize),
            "}"
        };

        var newLines = new List<string>();

        var importAdded = false;

        var constructorUpdated = false;

        foreach (var line in lines)
        {
            if (!line.StartsWith("import") && !importAdded)
            {
                newLines.Add("import { TranslateService } from '@ngx-translate/core';");

                importAdded = true;
            }

            newLines.Add(line);

            if (line.StartsWith("export class AppComponent {") && !constructorUpdated && createConstructor)
            {
                foreach (var newLine in ctor)
                {
                    newLines.Add(newLine.Indent(1, tabSize));
                }

                constructorUpdated = true;
            }

            if (line.Contains("constructor") && !constructorUpdated && !createConstructor)
            {
                newLines.RemoveAt(newLines.Count - 1);

                var newLine = line.Replace("constructor(", "constructor(private readonly _translateService: TranslateService, ");

                newLines.Add(newLine);

                newLines.Add($"_translateService.setDefaultLang(\"en\");".Indent(2, tabSize));
                newLines.Add($"_translateService.use(localStorage.getItem(\"currentLanguage\") || \"en\");".Indent(2, tabSize));

                constructorUpdated = true;
            }
        }

        _fileSystem.WriteAllLines(appComponentPath, newLines.ToArray());
    }

    public void LocalizeAdd(AngularProjectReferenceModel model, List<string> locales)
    {
        var workspaceDirectory = _fileSystem.GetDirectoryName(_fileProvider.Get("angular.json", model.ReferencedDirectory));

        _commandService.Start("ng add @angular/localize --force", workspaceDirectory);

        AddSupportedLocales(model, locales);
    }

    public void AddSupportedLocales(AngularProjectReferenceModel model, List<string> locales)
    {
        var angularJsonPath = _fileProvider.Get("angular.json", model.ReferencedDirectory);

        var angularJson = JObject.Parse(_fileSystem.ReadAllText(angularJsonPath));

        _fileSystem.CreateDirectory($"{GetProjectDirectory(model)}{Path.DirectorySeparatorChar}src{Path.DirectorySeparatorChar}locale");

        angularJson.AddSupportedLocales(model.Name, locales);

        _fileSystem.WriteAllText(angularJsonPath, JsonConvert.SerializeObject(angularJson, Formatting.Indented));
    }

    public void I18nExtract(AngularProjectReferenceModel model)
    {
        var localeDirectory = $"{GetProjectDirectory(model)}{Path.DirectorySeparatorChar}src{Path.DirectorySeparatorChar}locale{Path.DirectorySeparatorChar}";

        _commandService.Start($"ng extract-i18n --output-path {localeDirectory}", GetProjectDirectory(model));

        foreach (var locale in GetSupportedLocales(model))
        {
            _fileSystem.Copy($"{localeDirectory}messages.xlf", $"{localeDirectory}messages.{locale}.xlf");
        }

        _fileSystem.Delete($"{localeDirectory}messages.xlf");
    }

    public string GetProjectDirectory(AngularProjectReferenceModel model)
    {
        var angularJsonPath = _fileProvider.Get("angular.json", model.ReferencedDirectory);

        var angularJson = JObject.Parse(_fileSystem.ReadAllText(angularJsonPath));

        return $"{Path.GetDirectoryName(angularJsonPath)}{Path.DirectorySeparatorChar}{angularJson.GetProjectDirectory(model.Name)}";
    }

    public List<string> GetSupportedLocales(AngularProjectReferenceModel model)
    {
        var angularJsonPath = _fileProvider.Get("angular.json", model.ReferencedDirectory);

        var angularJson = JObject.Parse(_fileSystem.ReadAllText(angularJsonPath));

        return angularJson.GetSupportedLocales(model.Name);
    }

    public void PrettierAdd(string directory)
    {
        var workspaceDirectory = Path.GetDirectoryName(_fileProvider.Get("angular.json", directory));

        var packageJsonPath = $"{workspaceDirectory}{Path.DirectorySeparatorChar}package.json";

        var packageJson = JObject.Parse(_fileSystem.ReadAllText(packageJsonPath));

        _commandService.Start("npm install prettier npm-run-all husky pretty-quick -D", workspaceDirectory);

        _fileSystem.WriteAllText($"{workspaceDirectory}{Path.DirectorySeparatorChar}.prettierrc.json", JsonConvert.SerializeObject(new
        {
            tabWidth = 2,
            useTabs = false,
            printWidth = 120,
        }, Formatting.Indented));

        packageJson.AddScripts(new Dictionary<string, string>()
        {
            { "format:fix", "pretty-quick --staged" },
            { "precommit", "run-s format:fix lint" },
            { "lint", "ng lint" },
            { "format:check", "prettier --config ./.prettierrc.json --list-different \"projects/**/src/{app,environments,assets}/**/*{.ts,.js,.json,.css,.scss}\"" }
        });

        _fileSystem.WriteAllText(packageJsonPath, JsonConvert.SerializeObject(packageJson, Formatting.Indented));
    }

    public void BootstrapAdd(AngularProjectReferenceModel model)
    {
        var workspaceDirectory = Path.GetDirectoryName(_fileProvider.Get("angular.json", model.ReferencedDirectory));

        var jsonPath = $"{workspaceDirectory}{Path.DirectorySeparatorChar}angular.json";

        var json = JObject.Parse(_fileSystem.ReadAllText(jsonPath));

        _commandService.Start("npm install bootstrap", workspaceDirectory);

        json.AddStyle(model.Name, "node_modules\\bootstrap\\dist\\css\\bootstrap.min.css".Replace(Path.DirectorySeparatorChar, '/'));

        _fileSystem.WriteAllText(jsonPath, JsonConvert.SerializeObject(json, Formatting.Indented));
    }

    public void ModelCreate(string name, string directory, string properties = null)
    {
        var serviceName = "DashboardService";

        ClassModel classModel = _syntaxService.SolutionModel?.GetClass(name, serviceName); ;

        if (classModel == null)
        {
            classModel = new ClassModel(name);
        }

        var model = new TypeScriptTypeModel(name);

        foreach (var property in classModel.Properties)
        {
            model.Properties.Add(property.ToTs());
        }

        if (!string.IsNullOrEmpty(properties))
        {
            foreach (var property in properties.Split(','))
            {
                var parts = property.Split(':');
                var propertyName = parts[0];
                var propertyType = parts[1];

                model.Properties.Add(PropertyModel.TypeScriptProperty(propertyName, propertyType));
            }
        }

        var fileModel = new ObjectFileModel<TypeScriptTypeModel>(model, ((SyntaxToken)model.Name).SnakeCase(), directory, "ts");

        _artifactGenerator.CreateFor(fileModel);
    }

    public void ListComponentCreate(string name, string directory)
    {
        var nameSnakeCase = _namingConventionConverter.Convert(NamingConvention.SnakeCase, name);

        ComponentCreate($"{nameSnakeCase}-list", directory);
    }

    public void DetailComponentCreate(string name, string directory)
    {
        var nameSnakeCase = _namingConventionConverter.Convert(NamingConvention.SnakeCase, name);

        ComponentCreate($"{nameSnakeCase}-detail", directory);
    }

    public void IndexCreate(bool scss, string directory)
    {
        List<string> lines = new();

        foreach (var path in Directory.GetDirectories(directory))
        {
            var files = Directory.GetFiles(path);

            var fileNames = Directory.GetFiles(path).Select(Path.GetFileNameWithoutExtension);

            var containsIndex = Directory.GetFiles(path)
                .Select(Path.GetFileNameWithoutExtension)
            .Contains("index");

            if (!scss && Directory.GetFiles(path)
                .Select(Path.GetFileNameWithoutExtension)
                .Contains("index"))
            {
                lines.Add($"export * from './{Path.GetFileNameWithoutExtension(path)}';");
            }
        }
        if (scss)
        {

            foreach (var file in Directory.GetFiles(directory, "*.scss"))
            {
                if (!file.EndsWith("index.scss"))
                    lines.Add($"@use './{Path.GetFileNameWithoutExtension(file)}';");
            }

            _fileSystem.WriteAllLines($"{directory}{Path.DirectorySeparatorChar}index.scss", lines.ToArray());
        }
        else
        {
            foreach (var file in Directory.GetFiles(directory, "*.ts"))
            {
                if (!file.Contains(".spec.") && !file.EndsWith("index.ts"))
                    lines.Add($"export * from './{Path.GetFileNameWithoutExtension(file)}';");
            }

            _fileSystem.WriteAllLines($"{directory}{Path.DirectorySeparatorChar}index.ts", lines.ToArray());
        }
    }

    public void DefaultScssCreate(string directory)
    {
        var applicationDirectory = Path.GetDirectoryName(_fileProvider.Get("tsconfig.app.json", directory));

        var scssDirectory = Path.Combine(applicationDirectory, "src", "scss");

        _fileSystem.CreateDirectory(scssDirectory);

        foreach (var name in new string[] {
                "Actions",
                "Brand",
                "Breakpoints",
                "Buttons",
                "Dialogs",
                "Field",
                "FormFields",
                "Header",
                "Label",
                "Pills",
                "RouterLinkActive",
                "Table",
                "Textarea",
                "Title",
                "TitleBar",
                "Variables"
            })
        {
            var nameSnakeCase = _namingConventionConverter.Convert(NamingConvention.SnakeCase, name);

            var model = _fileModelFactory.CreateTemplate(name, $"_{nameSnakeCase}", scssDirectory, "scss", tokens: new TokensBuilder().With("prefix", "g").Build());

            _artifactGenerator.CreateFor(model);
        }

        IndexCreate(true, scssDirectory);
    }

    public void ScssComponentCreate(string name, string directory)
    {
        var applicationDirectory = Path.GetDirectoryName(_fileProvider.Get("tsconfig.app.json", directory));

        var scssDirectory = Path.Combine(applicationDirectory, "src", "scss");

        _fileSystem.CreateDirectory(scssDirectory);

        var nameSnakeCase = _namingConventionConverter.Convert(NamingConvention.SnakeCase, name);

        _fileSystem.WriteAllLines(Path.Combine(scssDirectory, $"_{nameSnakeCase}.scss"), new string[3] {
                    $".g-{nameSnakeCase}" + " {",
                    "",
                    "}"
                });

        IndexCreate(true, scssDirectory);
    }

    public void ControlCreate(string name, string directory)
    {
        var nameSnakeCase = ((SyntaxToken)name).SnakeCase();

        _commandService.Start($"ng g c {name}", directory);

        var componentDirectory = $"{directory}{Path.DirectorySeparatorChar}{nameSnakeCase}";

        _artifactGenerator.CreateFor(_fileModelFactory.CreateTemplate(
            "Components.Control.Component",
            $"{nameSnakeCase}.component",
            componentDirectory,
            "ts",
            tokens: new TokensBuilder()
            .With("name", name)
            .Build()
            ));

        _artifactGenerator.CreateFor(_fileModelFactory.CreateTemplate(
            "Components.Control.Html",
            $"{nameSnakeCase}.component",
            componentDirectory,
            "html",
            tokens: new TokensBuilder()
            .With("name", name)
            .With("messageBinding", "{{ vm.message }}")
            .Build()
            ));

        IndexCreate(false, componentDirectory);

        IndexCreate(false, directory);

        _observableNotifications.Broadcast(new FileCreated($"{componentDirectory}{Path.DirectorySeparatorChar}{nameSnakeCase}.component.ts"));

        _observableNotifications.Broadcast(new FileCreated($"{componentDirectory}{Path.DirectorySeparatorChar}{nameSnakeCase}.component.scss"));

        _observableNotifications.Broadcast(new FileCreated($"{componentDirectory}{Path.DirectorySeparatorChar}{nameSnakeCase}.component.html"));

        _observableNotifications.Broadcast(new FileCreated($"{componentDirectory}{Path.DirectorySeparatorChar}{nameSnakeCase}.component.spec.ts"));
    }

    public void Test(string directory, string searchPattern = "*.spec.ts")
    {
        var angularJsonPath = _fileProvider.Get("angular.json", directory);

        var angularJson = JObject.Parse(_fileSystem.ReadAllText(angularJsonPath));

        var projectDirectory = Path.GetDirectoryName(_fileProvider.Get("tsconfig.*", directory));

        string root = null!;

        var rootParts = new List<string>();

        var afterProjects = false;

        foreach (var part in projectDirectory.Split(Path.DirectorySeparatorChar).Skip(2))
        {
            if (afterProjects)
            {
                rootParts.Add(part);
            }

            if (part == "projects")
            {
                afterProjects = true;
            }
        }

        root = rootParts.Count > 1 ? $"@{string.Join("/", rootParts)}" : rootParts.First();

        foreach (var path in new DirectoryInfo(directory).GetFiles(searchPattern)
            .OrderByDescending(fileInfo => fileInfo.LastWriteTime)
            .Select(fileInfo => fileInfo.FullName))
        {
            var testName = string.Join(string.Empty, Path.GetFileNameWithoutExtension(path)
                .Remove(".spec")
                .Split('.').Select(x => _namingConventionConverter.Convert(NamingConvention.PascalCase, x)));

            _commandService.Start($"ng test --test-name-pattern='{testName}' --watch --project {root}", directory);
        }
    }
}

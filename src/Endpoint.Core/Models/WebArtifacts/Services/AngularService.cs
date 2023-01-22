using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Artifacts.Files;
using Endpoint.Core.Models.Artifacts.Files.Factories;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Endpoint.Core.Models.WebArtifacts.Services;

public class AngularService : IAngularService
{
    private readonly ILogger<AngularService> _logger;
    private readonly IArtifactGenerationStrategyFactory _artifactGenerationStrategyFactory;
    private readonly ICommandService _commandService;
    private readonly IFileProvider _fileProvider;
    private readonly IFileSystem _fileSystem;
    private readonly IFileModelFactory _fileModelFactory;
    public AngularService(
        ILogger<AngularService> logger,
        IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory,
        ICommandService commandService,
        IFileProvider fileProvider,
        IFileSystem fileSystem,
        IFileModelFactory fileModelFactory
        )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _artifactGenerationStrategyFactory = artifactGenerationStrategyFactory ?? throw new ArgumentNullException(nameof(artifactGenerationStrategyFactory));
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
        _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _fileModelFactory = fileModelFactory ?? throw new ArgumentNullException(nameof(fileModelFactory));
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

        _commandService.Start("npm install -D jest jest-preset-angular @angular-builders/jest @types/jest", workspaceDirectory);

        _fileSystem.WriteAllText($"{workspaceDirectory}{Path.DirectorySeparatorChar}setup-jest.ts", string.Empty);
    }


    public void CreateWorkspace(string name, string projectName, string projectType, string prefix, string rootDirectory)
    {
        var workspaceModel = new AngularWorkspaceModel(name, rootDirectory);

        _artifactGenerationStrategyFactory.CreateFor(workspaceModel);

        KarmaRemove(workspaceModel.Directory);

        JestInstall(workspaceModel.Directory);

        var angularProjectModel = new AngularProjectModel(projectName, projectType, prefix, workspaceModel.Directory);

        AddProject(angularProjectModel);

        _commandService.Start("code .", workspaceModel.Directory);
    }

    public void AddProject(AngularProjectModel model)
    {
        var stringBuilder = new StringBuilder().Append($"ng generate {model.ProjectType} {model.Name} --prefix {model.Prefix}");

        if(model.ProjectType == "application")
            stringBuilder.Append(" --style=scss --strict=false --routing");

        _commandService.Start(stringBuilder.ToString(), model.RootDirectory);


        if(model.ProjectType == "application")
        {
            EnableDefaultStandaloneComponents(model.RootDirectory, model.Name);

            var srcDirectory = $"{model.Directory}{Path.DirectorySeparatorChar}src{Path.DirectorySeparatorChar}";

            var appDirectory = $"{srcDirectory}app{Path.DirectorySeparatorChar}";

            
            var files = new List<FileModel>
            {
                _fileModelFactory.CreateTemplate("Angular.app.component","app.component", appDirectory, "ts"),
                _fileModelFactory.CreateTemplate("Angular.app.component.spec","app.component.spec", appDirectory, "ts"),
                _fileModelFactory.CreateTemplate("Angular.main","main", srcDirectory, "ts"),
            };

            foreach(var file in files) {
                _artifactGenerationStrategyFactory.CreateFor(file);
            }

            _fileSystem.Delete($"{appDirectory}{Path.DirectorySeparatorChar}app.module.ts");

        }


        UpdateCompilerOptionsToUseJestTypes(model);

        JestConfigCreate(model);

        UpdateAngularJsonToUseJest(model);

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

        stringBuilder.AppendLine("setupFilesAfterEnv: ['<rootDir>/setup-jest.ts'],".Indent(1));

        stringBuilder.AppendLine("globalSetup: 'jest-preset-angular/global-setup',".Indent(1));

        stringBuilder.AppendLine("};");

        _fileSystem.WriteAllText($"{model.Directory}{Path.DirectorySeparatorChar}jest.config.js",stringBuilder.ToString());

    }

    public void EnableDefaultStandaloneComponents(string directory, string projectName)
    {
        var angularJsonPath = _fileProvider.Get("angular.json", directory);

        var angularJson = JObject.Parse(_fileSystem.ReadAllText(angularJsonPath));

        angularJson.EnableDefaultStandaloneComponents(projectName);

        _fileSystem.WriteAllText(angularJsonPath, JsonConvert.SerializeObject(angularJson, Formatting.Indented));
    }


    public void UpdateCompilerOptionsToUseJestTypes(AngularProjectModel model)
    {        
        var tsConfigSpecJsonPath = $"{model.Directory}{Path.DirectorySeparatorChar}tsconfig.spec.json";

        var tsConfigSpecJson = JObject.Parse(File.ReadAllText(tsConfigSpecJsonPath));

        tsConfigSpecJson.UpdateCompilerOptionsToUseJestTypes();

        _fileSystem.WriteAllText(tsConfigSpecJsonPath, JsonConvert.SerializeObject(tsConfigSpecJson, Formatting.Indented));
    }
}


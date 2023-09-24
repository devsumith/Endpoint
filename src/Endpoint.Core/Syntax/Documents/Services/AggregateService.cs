// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.IO;
using System.Linq;
using Endpoint.Core.Artifacts;
using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Artifacts.Files.Factories;
using Endpoint.Core.Artifacts.Projects.Services;
using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax.Classes.Factories;
using Endpoint.Core.Syntax.Documents.Factories;
using Microsoft.Extensions.Logging;
using static Endpoint.Core.Constants.FileExtensions;

namespace Endpoint.Core.Syntax.Documents.Services;

public class AggregateService : IAggregateService
{
    private readonly ILogger<AggregateService> logger;
    private readonly INamingConventionConverter namingConventionConverter;
    private readonly ICodeAnalysisService codeAnalysisService;
    private readonly IArtifactGenerator artifactGenerator;
    private readonly IClassFactory classFactory;
    private readonly IProjectService projectService;
    private readonly IFileFactory fileFactory;
    private readonly IFileProvider fileProvider;
    private readonly ISyntaxUnitFactory syntaxUnitFactory;
    private readonly ICqrsFactory cqrsFactory;

    public AggregateService(
        ILogger<AggregateService> logger,
        INamingConventionConverter namingConventionConverter,
        ICodeAnalysisService codeAnalysisService,
        IArtifactGenerator artifactGenerator,
        IClassFactory classFactory,
        IProjectService projectService,
        IFileFactory fileFactory,
        IFileProvider fileProvider,
        ISyntaxUnitFactory syntaxUnitFactory,
        ICqrsFactory cqrsFactory)
    {
        this.codeAnalysisService = codeAnalysisService ?? throw new ArgumentNullException(nameof(codeAnalysisService));
        this.namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
        this.classFactory = classFactory ?? throw new ArgumentNullException(nameof(classFactory));
        this.projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
        this.fileFactory = fileFactory ?? throw new ArgumentNullException(nameof(fileFactory));
        this.fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        this.syntaxUnitFactory = syntaxUnitFactory ?? throw new ArgumentException(nameof(syntaxUnitFactory));
        this.cqrsFactory = cqrsFactory ?? throw new ArgumentNullException(nameof(cqrsFactory));
    }

    public async Task<ClassModel> AddAsync(string name, string properties, string directory, string serviceName)
    {
        logger.LogInformation("Add");

        EnsureCorePackagesAreInstalled(directory);

        EnsureCoreFilesAreAdded(directory);

        if (string.IsNullOrEmpty(serviceName))
        {
            var projectPath = fileProvider.Get("*.csproj", directory);

            serviceName = Path.GetFileNameWithoutExtension(projectPath).Split('.').First();
        }

        var classModel = codeAnalysisService.SolutionModel?.GetClass(name, serviceName);

        if (classModel == null)
        {
            classModel = await classFactory.CreateEntityAsync(name, properties);
        }

        var model = await syntaxUnitFactory.CreateAsync(name, classModel.Properties);

        await artifactGenerator.GenerateAsync(model);

        var fileModel = fileFactory.CreateDbContextInterface(directory);

        await artifactGenerator.GenerateAsync(fileModel);

        return classModel;
    }

    [Obsolete("Use Folder Factory")]
    public async Task CommandCreateAsync(string routeType, string name, string aggregate, string properties, string directory)
    {
        var serviceName = Path.GetFileNameWithoutExtension(fileProvider.Get("*.csproj", directory)).Split('.').First();

        var classModel = codeAnalysisService.SolutionModel?.GetClass(aggregate, serviceName);

        if (classModel == null)
        {
            classModel = await classFactory.CreateEntityAsync(aggregate, properties);
        }

        var commandModel = new CommandModel(); // (serviceName, classModel, _namingConventionConverter, name: name, routeType: routeType switch
        /*        {
                    "create" => RouteType.Create,
                    "update" => RouteType.Update,
                    "delete" => RouteType.Delete,
                    _ => throw new NotSupportedException()
                })*/

        var model = new CodeFileModel<CommandModel>(commandModel, commandModel.UsingDirectives, commandModel.Name, directory, ".cs");

        await artifactGenerator.GenerateAsync(model);
    }

    public async Task QueryCreateAsync(string routeType, string name, string aggregate, string properties, string directory)
    {
        var rootNamespace = Path.GetFileNameWithoutExtension(fileProvider.Get("*.csproj", directory)).Split('.').First();

        var queryModel = await cqrsFactory.CreateQueryAsync(routeType, name, properties);

        var model = new CodeFileModel<QueryModel>(queryModel, queryModel.UsingDirectives, queryModel.Name, directory, CSharpFile);

        await artifactGenerator.GenerateAsync(model);
    }

    private void EnsureCorePackagesAreInstalled(string directory)
    {
        projectService.PackageAdd("MediatR", directory);
        projectService.PackageAdd("FluentValidation", directory);
        projectService.PackageAdd("Microsoft.EntityFrameworkCore", directory);
        projectService.PackageAdd("Microsoft.Extensions.Logging.Abstractions", directory);
    }

    private void EnsureCoreFilesAreAdded(string directory)
    {
        projectService.CoreFilesAdd(directory);
    }
}

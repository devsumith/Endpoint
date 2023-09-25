// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Endpoint.Core.Artifacts.Folders.Factories;
using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Classes;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Artifacts.Folders.Services;

public class FolderService : IFolderService
{
    private readonly ILogger<FolderService> logger;
    private readonly IFolderFactory folderFactory;
    private readonly IArtifactGenerator artifactGenerator;
    private readonly ICodeAnalysisService codeAnalysisService;
    private readonly IFileProvider fileProvider;

    public FolderService(
        ILogger<FolderService> logger,
        IFolderFactory folderFactory,
        IArtifactGenerator artifactGenerator,
        ICodeAnalysisService codeAnalysisService,
        IFileProvider fileProvider)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.folderFactory = folderFactory ?? throw new ArgumentNullException(nameof(folderFactory));
        this.artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
        this.codeAnalysisService = codeAnalysisService ?? throw new ArgumentNullException(nameof(codeAnalysisService));
        this.fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
    }

    public async Task<FolderModel> AggregateQueries(ClassModel aggregate, string directory)
    {
        var model = await folderFactory.CreateAggregateQueriesAsync(aggregate);

        var serviceName = Path.GetFileNameWithoutExtension(fileProvider.Get("*.csproj", directory).Split('.').First());

        var entity = codeAnalysisService.SolutionModel?.GetClass(aggregate.Name, serviceName);

        await artifactGenerator.GenerateAsync(model);

        return model;
    }

    public async Task<FolderModel> AggregateCommands(ClassModel aggregate, string directory)
    {
        var model = await folderFactory.CreateAggregateCommandsAsync(aggregate, directory);

        var serviceName = Path.GetFileNameWithoutExtension(fileProvider.Get("*.csproj", directory).Split('.').First());

        var entity = codeAnalysisService.SolutionModel?.GetClass(aggregate.Name, serviceName);

        await artifactGenerator.GenerateAsync(model);

        return model;
    }
}
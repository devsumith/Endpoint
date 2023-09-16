// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.IO;
using System.Threading.Tasks;
using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Artifacts.Services;

public class SignalRService : ISignalRService
{
    private readonly ILogger<SignalRService> logger;
    private readonly IFileProvider fileProvider;
    private readonly ICommandService commandService;
    private readonly IArtifactGenerator artifactGenerator;

    public SignalRService(
        ILogger<SignalRService> logger,
        IFileProvider fileProvider,
        ICommandService commandService,
        IArtifactGenerator artifactGenerator)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        this.artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
        this.commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
    }

    public async Task Add(string directory)
    {
        var workspaceDirectory = Path.GetDirectoryName(fileProvider.Get("package.json", directory));

        commandService.Start("npm install -D @microsoft/signalr", workspaceDirectory);
    }

    public async Task AddHub(string name, string directory)
    {
        var projectDirectory = Path.GetDirectoryName(fileProvider.Get("*.csproj", directory));

        await artifactGenerator.GenerateAsync(CreateSignalRHubFileModel(name, directory));

        await artifactGenerator.GenerateAsync(CreateSignalRHubFileModel(name, directory));
    }

    public FileModel CreateSignalRHubFileModel(string name, string directory)
    {
        var model = new SignalrHUbModel(name);

        throw new NotImplementedException();
    }

    public FileModel CreateSignalRHubInterfaceFileModel(string name, string directory)
    {
        // var model = new SignalrHUbInterfaceModel(name);
        throw new NotImplementedException();
    }
}

public class SignalrHUbModel
{
    public SignalrHUbModel(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
}

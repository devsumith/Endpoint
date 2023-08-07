// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

namespace Endpoint.Core.Artifacts.Services;

public class SignalRService : ISignalRService
{
    private readonly ILogger<SignalRService> _logger;
    private readonly IFileProvider _fileProvider;
    private readonly ICommandService _commandService;
    private readonly IArtifactGenerator _artifactGenerator;
    public SignalRService(
        ILogger<SignalRService> logger,
        IFileProvider fileProvider,
        ICommandService commandService,
        IArtifactGenerator artifactGenerator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        _artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
    }

    public async Task Add(string directory)
    {
        var workspaceDirectory = Path.GetDirectoryName(_fileProvider.Get("package.json", directory));

        _commandService.Start("npm install -D @microsoft/signalr", workspaceDirectory);
    }

    public async Task AddHub(string name, string directory)
    {
        var projectDirectory = Path.GetDirectoryName(_fileProvider.Get("*.csproj", directory));

        await _artifactGenerator.GenerateAsync(CreateSignalRHubFileModel(name, directory));

        await _artifactGenerator.GenerateAsync(CreateSignalRHubFileModel(name, directory));
    }

    public FileModel CreateSignalRHubFileModel(string name, string directory)
    {
        var model = new SignalrHUbModel(name);

        throw new NotImplementedException();
    }

    public FileModel CreateSignalRHubInterfaceFileModel(string name, string directory)
    {
        //var model = new SignalrHUbInterfaceModel(name);

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

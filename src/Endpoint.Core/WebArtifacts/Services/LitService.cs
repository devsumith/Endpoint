// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;

using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Endpoint.Core.WebArtifacts.Services;

public class LitService : ILitService
{
    private readonly ILogger<LitService> _logger;
    private readonly IFileSystem fileSystem;
    private readonly IArtifactGenerator _artifactGenerator;

    public LitService(
        ILogger<LitService> logger,
        IArtifactGenerator artifactGenerator,
        IFileSystem fileSystem)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
    }

    public async Task WorkspaceCreate(string name, string rootDirectory)
    {
        var model = new LitWorkspaceModel(name, rootDirectory);

        await _artifactGenerator.CreateAsync(model);
    }

    public async Task ProjectCreate(string name, string rootDirectory)
    {
        var projectModel = new LitProjectModel(name, rootDirectory);

        await _artifactGenerator.CreateAsync(projectModel);

        WorkspaceAdd(projectModel);
    }

    private void WorkspaceAdd(LitProjectModel model)
    {

    }

}



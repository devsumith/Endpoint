// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Endpoint.Core.Artifacts.Lit;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Artifacts.Services;

public class LitService : ILitService
{
    private readonly ILogger<LitService> logger;
    private readonly IFileSystem fileSystem;
    private readonly IArtifactGenerator artifactGenerator;

    public LitService(
        ILogger<LitService> logger,
        IArtifactGenerator artifactGenerator,
        IFileSystem fileSystem)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
    }

    public async Task WorkspaceCreate(string name, string rootDirectory)
    {
        var model = new LitWorkspaceModel(name, rootDirectory);

        await artifactGenerator.GenerateAsync(model);
    }

    public async Task ProjectCreate(string name, string rootDirectory)
    {
        var projectModel = new LitProjectModel(name, rootDirectory);

        await artifactGenerator.GenerateAsync(projectModel);

        WorkspaceAdd(projectModel);
    }

    private void WorkspaceAdd(LitProjectModel model)
    {
    }
}

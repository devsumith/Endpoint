// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.IO;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Artifacts.Workspaces;

public class AngularWorkspaceGenerationStrategy : GenericArtifactGenerationStrategy<AngularWorkspaceModel>
{
    private readonly ILogger<AngularWorkspaceGenerationStrategy> logger;
    private readonly ICommandService commandService;

    public AngularWorkspaceGenerationStrategy(ILogger<AngularWorkspaceGenerationStrategy> logger, ICommandService commandService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
    }

    public override async Task GenerateAsync(IArtifactGenerator artifactGenerator, AngularWorkspaceModel model)
    {
        logger.LogInformation("Generating Angular Workspace. {name}", model.Name);

        commandService.Start($"npm uninstall @angular/cli -g", model.RootDirectory);

        commandService.Start($"npm install @angular/cli@{model.Version} -g", model.RootDirectory);

        commandService.Start($"ng new {model.Name} --no-create-application", model.RootDirectory);

        commandService.Start($"npm install @ngrx/component-store --force", Path.Combine(model.RootDirectory, model.Name));

        commandService.Start($"npm install @ngrx/component --force", Path.Combine(model.RootDirectory, model.Name));
    }
}

// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;
using System.IO;

namespace Endpoint.Core.Models.WebArtifacts.Strategies;

public class AngularWorkspaceArtifactGenerationStrategy : ArtifactGenerationStrategyBase<AngularWorkspaceModel>
{
    private readonly ILogger<AngularWorkspaceArtifactGenerationStrategy> _logger;
    private readonly ICommandService _commandService;

    public AngularWorkspaceArtifactGenerationStrategy(
        IServiceProvider serviceProvider,
        ILogger<AngularWorkspaceArtifactGenerationStrategy> logger,
        ICommandService commandService)
        : base(serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
    }

    public override void Create(IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory, AngularWorkspaceModel model, dynamic context = null)
    {
        _logger.LogInformation("Generating artifact for {0}.", model);

        _commandService.Start($"npm uninstall @angular/cli -g", model.RootDirectory);

        _commandService.Start($"npm install @angular/cli -g", model.RootDirectory);

        _commandService.Start($"ng new {model.Name} --no-create-application", model.RootDirectory);

        _commandService.Start($"npm install @ngrx/component-store --force", Path.Combine(model.RootDirectory, model.Name));

        _commandService.Start($"npm install @ngrx/component --force", Path.Combine(model.RootDirectory, model.Name));

    }
}

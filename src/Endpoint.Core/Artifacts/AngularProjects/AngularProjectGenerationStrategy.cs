// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Artifacts.AngularProjects;

public class AngularProjectGenerationStrategy : GenericArtifactGenerationStrategy<AngularProjectModel>
{
    private readonly ICommandService _commandService;
    private readonly ILogger<AngularProjectGenerationStrategy> _logger;
    public AngularProjectGenerationStrategy(
        ICommandService commandService,
        ILogger<AngularProjectGenerationStrategy> logger)
    {
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public int Priority => throw new NotImplementedException();

    public bool CanHandle(AngularProjectModel model, dynamic context = null) => model is AngularProjectModel;

    public override async Task GenerateAsync(IArtifactGenerator generator, AngularProjectModel model, dynamic context = null)
    {
        _logger.LogInformation("Create Angular Project. {name}", model.Name);

        _commandService.Start($"ng new {model.Name}", model.Directory);
    }
}



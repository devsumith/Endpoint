// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Artifacts.React;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Artifacts.Services;

public class ReactService : IReactService
{
    private readonly ILogger<ReactService> logger;
    private readonly ICommandService commandService;

    public ReactService(ILogger<ReactService> logger, ICommandService commandService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
    }

    public void Create(ReactAppReferenceModel model)
    {
        commandService.Start($"npx create-react-app {model.Name} --template typescript", model.ReferenceDirectory);
    }
}

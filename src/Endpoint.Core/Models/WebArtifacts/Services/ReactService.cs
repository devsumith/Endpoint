using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Models.WebArtifacts.Services;

public class ReactService: IReactService
{
    private readonly ILogger<ReactService> _logger;
    private readonly ICommandService _commandService;

    public ReactService(ILogger<ReactService> logger, ICommandService commandService){
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
    }

    public void Create(ReactAppReferenceModel model)
    {
        _commandService.Start($"npx create-react-app {model.Name} --template typescript", model.ReferenceDirectory);
    }

}


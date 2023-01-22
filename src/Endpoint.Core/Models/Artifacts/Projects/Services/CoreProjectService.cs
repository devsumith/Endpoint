using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Endpoint.Core.Models.Artifacts.Projects.Services;

public class CoreProjectService: ICoreProjectService
{
    private readonly ILogger<CoreProjectService> _logger;

    public CoreProjectService(ILogger<CoreProjectService> logger){
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task DoWorkAsync()
    {
        _logger.LogInformation("DoWorkAsync");
    }

}


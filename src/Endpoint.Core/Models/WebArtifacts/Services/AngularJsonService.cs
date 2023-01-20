using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Endpoint.Core.Models.WebArtifacts.Services;

public class AngularJsonService: IAngularJsonService
{
    private readonly ILogger<AngularJsonService> _logger;

    public AngularJsonService(ILogger<AngularJsonService> logger){
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task DoWorkAsync()
    {
        _logger.LogInformation("DoWorkAsync");
    }

}


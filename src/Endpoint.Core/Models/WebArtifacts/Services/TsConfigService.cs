using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Endpoint.Core.Models.WebArtifacts.Services;

public class TsConfigService: ITsConfigService
{
    private readonly ILogger<TsConfigService> _logger;

    public TsConfigService(ILogger<TsConfigService> logger){
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task DoWorkAsync()
    {
        _logger.LogInformation("DoWorkAsync");
    }

}


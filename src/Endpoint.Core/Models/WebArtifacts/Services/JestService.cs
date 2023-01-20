using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Endpoint.Core.Models.WebArtifacts.Services;

public class JestService: IJestService
{
    private readonly ILogger<JestService> _logger;

    public JestService(ILogger<JestService> logger){
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task DoWorkAsync()
    {
        _logger.LogInformation("DoWorkAsync");
    }

}


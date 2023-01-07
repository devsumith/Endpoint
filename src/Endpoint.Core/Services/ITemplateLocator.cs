using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Services;

public interface ITemplateLocator
{
    string[] Get(string filename);
}

public class EmptyTemplateLocator : ITemplateLocator
{
    private readonly ILogger<EmptyTemplateLocator> _logger;
    public EmptyTemplateLocator(ILogger<EmptyTemplateLocator> logger)
    {
        _logger = logger;
    }
    public string[] Get(string filename)
    {
        _logger.LogCritical("Register a template locator: {0}", filename);

        return new string[0];
    }
}
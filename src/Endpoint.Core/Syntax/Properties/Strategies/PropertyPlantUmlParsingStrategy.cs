using Microsoft.Extensions.Logging;
using System.Text;
using Endpoint.Core.Services;

namespace Endpoint.Core.Syntax.Properties.Strategies;

public class PropertyPlantUmlParsingStrategy : BaseSyntaxParsingStrategy<PropertyModel>
{
    private readonly ILogger<PropertyPlantUmlParsingStrategy> _logger;
    private readonly IContext _context;

    public PropertyPlantUmlParsingStrategy(IContext context, ILogger<PropertyPlantUmlParsingStrategy> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task<PropertyModel> ParseAsync(ISyntaxParser parser, string value)
    {
        _logger.LogInformation("Parsing PlantUml for syntax. {typeName}", typeof(PropertyModel).Name);

        throw new NotImplementedException();
    }
}
using Endpoint.Core.Abstractions;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Endpoint.Core.Models.Syntax.Properties;

internal class PropertySyntaxGenerationStrategy : SyntaxGenerationStrategyBase<PropertyModel>
{
    private readonly ILogger<PropertySyntaxGenerationStrategy> _logger;
    public PropertySyntaxGenerationStrategy(
        IServiceProvider serviceProvider,
        ILogger<PropertySyntaxGenerationStrategy> logger) 
        : base(serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override string Create(ISyntaxGenerationStrategyFactory syntaxGenerationStrategyFactory, PropertyModel model, dynamic configuration = null)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();


        return builder.ToString();
    }
}
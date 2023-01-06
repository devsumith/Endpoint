using Endpoint.Core.Abstractions;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Endpoint.Core.Models.Syntax.Attributes;

internal class AttributeSyntaxGenerationStrategy : SyntaxGenerationStrategyBase<AttributeModel>
{
    private readonly ILogger<AttributeSyntaxGenerationStrategy> _logger;
    public AttributeSyntaxGenerationStrategy(
        IServiceProvider serviceProvider,
        ILogger<AttributeSyntaxGenerationStrategy> logger) 
        : base(serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override string Create(ISyntaxGenerationStrategyFactory syntaxGenerationStrategyFactory, AttributeModel model, dynamic configuration = null)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();


        return builder.ToString();
    }
}
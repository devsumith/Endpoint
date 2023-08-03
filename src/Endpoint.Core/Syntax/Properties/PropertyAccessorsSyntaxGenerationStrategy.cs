using Endpoint.Core.Abstractions;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text;

namespace Endpoint.Core.Syntax.Properties;

public class PropertyAccessorsSyntaxGenerationStrategy : SyntaxGenerationStrategyBase<List<PropertyAccessorModel>>
{
    private readonly ILogger<PropertyAccessorsSyntaxGenerationStrategy> _logger;
    public PropertyAccessorsSyntaxGenerationStrategy(
        IServiceProvider serviceProvider,
        ILogger<PropertyAccessorsSyntaxGenerationStrategy> logger)
        : base(serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task<string> CreateAsync(ISyntaxGenerator syntaxGenerator, List<PropertyAccessorModel> model, dynamic context = null)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        builder.Append("{ get; set; }");

        return builder.ToString();
    }
}
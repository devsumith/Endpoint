using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text;

namespace Endpoint.Core.Syntax.Properties.Strategies;

public class PropertyAccessorsSyntaxGenerationStrategy : GenericSyntaxGenerationStrategy<List<PropertyAccessorModel>>
{
    private readonly ILogger<PropertyAccessorsSyntaxGenerationStrategy> _logger;
    public PropertyAccessorsSyntaxGenerationStrategy(
        IServiceProvider serviceProvider,
        ILogger<PropertyAccessorsSyntaxGenerationStrategy> logger)

    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    public int GetPriority() => 0;


    public override async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, List<PropertyAccessorModel> model, dynamic context = null)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        builder.Append("{ get; set; }");

        return builder.ToString();
    }
}
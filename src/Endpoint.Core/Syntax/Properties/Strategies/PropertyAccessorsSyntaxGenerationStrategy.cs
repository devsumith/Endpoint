using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Syntax.Properties.Strategies;

public class PropertyAccessorsSyntaxGenerationStrategy : GenericSyntaxGenerationStrategy<List<PropertyAccessorModel>>
{
    private readonly ILogger<PropertyAccessorsSyntaxGenerationStrategy> logger;

    public PropertyAccessorsSyntaxGenerationStrategy(

        ILogger<PropertyAccessorsSyntaxGenerationStrategy> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public int GetPriority() => 0;

    public override async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, List<PropertyAccessorModel> model)
    {
        logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        builder.Append("{ get; set; }");

        return builder.ToString();
    }
}
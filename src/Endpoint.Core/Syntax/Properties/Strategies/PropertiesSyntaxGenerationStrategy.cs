using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Endpoint.Core.Syntax.Properties.Strategies;

public class PropertiesSyntaxGenerationStrategy : GenericSyntaxGenerationStrategy<List<PropertyModel>>
{
    private readonly ILogger<PropertiesSyntaxGenerationStrategy> _logger;
    public PropertiesSyntaxGenerationStrategy(
        IServiceProvider serviceProvider,
        ILogger<PropertiesSyntaxGenerationStrategy> logger)

    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public int GetPriority() => 0;


    public override async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, List<PropertyModel> model, dynamic context = null)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        foreach (var property in model)
        {
            builder.Append(await syntaxGenerator.GenerateAsync(property));

            if (property != model.Last())
                builder.AppendLine();
        }

        return builder.ToString();
    }
}
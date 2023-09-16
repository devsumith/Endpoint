using Microsoft.Extensions.Logging;
using System.Text;

namespace Endpoint.Core.Syntax.Properties.Strategies;

public class PropertySyntaxGenerationStrategy : GenericSyntaxGenerationStrategy<PropertyModel>
{
    private readonly ILogger<PropertySyntaxGenerationStrategy> _logger;
    public PropertySyntaxGenerationStrategy(

        ILogger<PropertySyntaxGenerationStrategy> logger)

    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public int GetPriority() => 0;

    public override async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, PropertyModel model)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        if (model.IsClassProperty)
        {
            builder.Append(await syntaxGenerator.GenerateAsync(model.AccessModifier));

            builder.Append(" ");

            if (model.Required)
            {
                builder.Append("required ");
            }
        }

        builder.Append($"{await syntaxGenerator.GenerateAsync(model.Type)} {model.Name} {await syntaxGenerator.GenerateAsync(model.Accessors)}");

        if (model.IsClassProperty && !string.IsNullOrEmpty(model.DefaultValue))
        {
            builder.Append($" = {model.DefaultValue};");
        }

        return builder.ToString();
    }
}

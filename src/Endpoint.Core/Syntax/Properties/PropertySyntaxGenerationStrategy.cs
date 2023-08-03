using Endpoint.Core.Abstractions;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Endpoint.Core.Syntax.Properties;

public class PropertySyntaxGenerationStrategy : SyntaxGenerationStrategyBase<PropertyModel>
{
    private readonly ILogger<PropertySyntaxGenerationStrategy> _logger;
    public PropertySyntaxGenerationStrategy(
        IServiceProvider serviceProvider,
        ILogger<PropertySyntaxGenerationStrategy> logger)
        : base(serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task<string> CreateAsync(ISyntaxGenerator syntaxGenerator, PropertyModel model, dynamic context = null)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        if (model.IsClassProperty)
        {
            builder.Append(await syntaxGenerator.CreateAsync(model.AccessModifier));

            builder.Append(" ");

            if (model.Required)
            {
                builder.Append("required ");
            }
        }

        builder.Append($"{await syntaxGenerator.CreateAsync(model.Type)} {model.Name} {await syntaxGenerator.CreateAsync(model.Accessors)}");

        if (model.IsClassProperty && !string.IsNullOrEmpty(model.DefaultValue))
        {
            builder.Append($" = {model.DefaultValue};");
        }

        return builder.ToString();
    }
}

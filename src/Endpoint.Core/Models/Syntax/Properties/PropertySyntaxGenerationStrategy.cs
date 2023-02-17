using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Syntax.Classes;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Endpoint.Core.Models.Syntax.Properties;

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

    public override string Create(ISyntaxGenerationStrategyFactory syntaxGenerationStrategyFactory, PropertyModel model, dynamic context = null)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        if (model.Parent is ClassModel)
        {
            builder.Append(syntaxGenerationStrategyFactory.CreateFor(model.AccessModifier));

            builder.Append(" ");
        }

        builder.Append($"{syntaxGenerationStrategyFactory.CreateFor(model.Type)} {model.Name} {syntaxGenerationStrategyFactory.CreateFor(model.Accessors)}");

        if (model.Parent is ClassModel && !string.IsNullOrEmpty(model.DefaultValue))
        {
            builder.Append($" = {model.DefaultValue};");
        }

        return builder.ToString();
    }
}

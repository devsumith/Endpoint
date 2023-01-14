using Endpoint.Core.Abstractions;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text;

namespace Endpoint.Core.Models.Syntax.Interfaces;

public class InterfaceSyntaxGenerationStrategy : SyntaxGenerationStrategyBase<InterfaceModel>
{
    private readonly ILogger<InterfaceSyntaxGenerationStrategy> _logger;
    public InterfaceSyntaxGenerationStrategy(
        IServiceProvider serviceProvider,
        ILogger<InterfaceSyntaxGenerationStrategy> logger) 
        : base(serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override bool CanHandle(object model, dynamic configuration = null)
    {
        return model as InterfaceModel != null;
    }
    public override string Create(ISyntaxGenerationStrategyFactory syntaxGenerationStrategyFactory, InterfaceModel model, dynamic configuration = null)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();
        
        builder.Append($"public interface {model.Name}");

        if (model.Implements.Count > 0)
        {
            builder.Append(": ");

            builder.Append(string.Join(',', model.Implements.Select(x => syntaxGenerationStrategyFactory.CreateFor(x, configuration))));
        }

        if (model.Properties.Count + model.Methods.Count == 0)
        {
            builder.Append(" { }");

            return builder.ToString();
        }

        builder.AppendLine($"");

        builder.AppendLine("{");

        if (model.Properties.Count > 0)
            builder.AppendLine(((string)syntaxGenerationStrategyFactory.CreateFor(model.Properties, configuration)).Indent(1));

        if (model.Methods.Count > 0)
            builder.AppendLine(((string)syntaxGenerationStrategyFactory.CreateFor(model.Methods, configuration)).Indent(1));

        builder.AppendLine("}");

        return builder.ToString();

    }
}
using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Syntax.Constructors;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text;

namespace Endpoint.Core.Models.Syntax.Methods;

public class MethodSyntaxGenerationStrategy : SyntaxGenerationStrategyBase<MethodModel>
{
    private readonly ILogger<MethodSyntaxGenerationStrategy> _logger;
    public MethodSyntaxGenerationStrategy(
        IServiceProvider serviceProvider,
        ILogger<MethodSyntaxGenerationStrategy> logger) 
        : base(serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override bool CanHandle(object model, dynamic configuration = null)
        => model is MethodModel methodModel && !methodModel.Interface;
    public override string Create(ISyntaxGenerationStrategyFactory syntaxGenerationStrategyFactory, MethodModel model, dynamic configuration = null)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        builder.Append(syntaxGenerationStrategyFactory.CreateFor(model.AccessModifier));

        if (model.Override)
            builder.Append(" override");

        if (model.Async)
            builder.Append(" async");

        if (model.Params.SingleOrDefault(x => x.ExtensionMethodParam) != null || model.Static)
            builder.Append(" static");

        builder.Append($" {syntaxGenerationStrategyFactory.CreateFor(model.ReturnType)}");

        builder.Append($" {model.Name}");

        builder.Append('(');

        builder.Append(string.Join(',', model.Params.Select(x => syntaxGenerationStrategyFactory.CreateFor(x))));

        builder.Append(')');

        if (string.IsNullOrEmpty(model.Body))
            builder.Append("{ }");
        else
        {
            builder.AppendLine();

            builder.AppendLine("{");

            builder.AppendLine(model.Body.Indent(1));

            builder.Append('}');
        }

        return builder.ToString();
    }
}

using Endpoint.Core.Abstractions;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text;

namespace Endpoint.Core.Models.Syntax.Methods;

public class InterfaceMethodSyntaxGenerationStrategy : SyntaxGenerationStrategyBase<MethodModel>
{
    private readonly ILogger<MethodSyntaxGenerationStrategy> _logger;
    public InterfaceMethodSyntaxGenerationStrategy(
        IServiceProvider serviceProvider,
        ILogger<MethodSyntaxGenerationStrategy> logger)
        : base(serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override bool CanHandle(object model, dynamic configuration = null)
    {
        return model is MethodModel methodModel && methodModel.Interface;
    }

    public override string Create(ISyntaxGenerationStrategyFactory syntaxGenerationStrategyFactory, MethodModel model, dynamic configuration = null)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        builder.Append($" {syntaxGenerationStrategyFactory.CreateFor(model.ReturnType)}");

        builder.Append($" {model.Name}");

        builder.Append('(');

        builder.Append(string.Join(',', model.Params.Select(x => syntaxGenerationStrategyFactory.CreateFor(x))));

        builder.Append(");");

        return builder.ToString();
    }
}
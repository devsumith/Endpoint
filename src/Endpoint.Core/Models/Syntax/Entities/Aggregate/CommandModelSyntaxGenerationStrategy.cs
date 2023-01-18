using Endpoint.Core.Abstractions;
using System.Text;

namespace Endpoint.Core.Models.Syntax.Entities.Aggregate;

public class CommandModelSyntaxGenerationStrategy : SyntaxGenerationStrategyBase<CommandModel>
{
    public CommandModelSyntaxGenerationStrategy(IServiceProvider serviceProvider) 
        :base(serviceProvider) { }

    public override string Create(ISyntaxGenerationStrategyFactory syntaxGenerationStrategyFactory, CommandModel model, dynamic context = null)
    {
        var builder = new StringBuilder();

        builder.AppendLine(syntaxGenerationStrategyFactory.CreateFor(model.RequestValidator));

        builder.AppendLine("");

        builder.AppendLine(syntaxGenerationStrategyFactory.CreateFor(model.Request));

        builder.AppendLine("");

        builder.AppendLine(syntaxGenerationStrategyFactory.CreateFor(model.Response));

        builder.AppendLine("");

        builder.AppendLine(syntaxGenerationStrategyFactory.CreateFor(model.RequestHandler, context));

        return builder.ToString();
    }
}
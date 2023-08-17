using Microsoft.Extensions.Logging;
using System.Text;

namespace Endpoint.Core.Syntax.Statements;

public class LogStatementSyntaxGenerationStrategy : GenericSyntaxGenerationStrategy<LogStatementModel>
{
    private readonly ILogger<LogStatementSyntaxGenerationStrategy> _logger;
    public LogStatementSyntaxGenerationStrategy(
        IServiceProvider serviceProvider,
        ILogger<LogStatementSyntaxGenerationStrategy> logger)

    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }



    public override async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, LogStatementModel model, dynamic context = null)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        var syntax = model.RouteType switch
        {
            RouteType.Create => BuildForCreateCommand(model),
            RouteType.Update => BuildForUpdateCommand(model),
            RouteType.Delete => BuildForDeleteCommand(model),
            _ => new string[0]
        };

        builder.AppendJoin(Environment.NewLine, syntax);

        return builder.ToString();
    }

    public string[] BuildForCreateCommand(LogStatementModel model)
    => new string[4]
    {
            "_logger.LogInformation(",
            "\"----- Sending command: {CommandName}: ({@Command})\",".Indent(1),
            $"nameof(Create{((SyntaxToken)model.Resource).PascalCase}Request),".Indent(1),
            "request);".Indent(1)
    };

    public string[] BuildForUpdateCommand(LogStatementModel model)
        => new string[]
        {
            "_logger.LogInformation(",
            "\"----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})\",".Indent(1),
            $"nameof(Update{((SyntaxToken)model.Resource).PascalCase}Request),".Indent(1),
            //$"nameof(request.{((SyntaxToken)model.Resource).PascalCase}.{IdPropertyNameBuilder.Build(model.Settings,model.Resource)}),".Indent(1),
            //$"request.{((SyntaxToken)model.Resource).PascalCase}.{IdPropertyNameBuilder.Build(model.Settings,model.Resource)},".Indent(1),
            "request);".Indent(1)
        };

    public string[] BuildForDeleteCommand(LogStatementModel model)
        => new string[]
        {
            "_logger.LogInformation(",
            "\"----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})\",".Indent(1),
            $"nameof(Remove{((SyntaxToken)model.Resource).PascalCase}Request),".Indent(1),
            //$"nameof(request.{IdPropertyNameBuilder.Build(model.Settings,model.Resource)}),".Indent(1),
            //$"request.{IdPropertyNameBuilder.Build(model.Settings,model.Resource)},".Indent(1),
            "request);".Indent(1)
        };

}
using Endpoint.Core.Abstractions;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text;

namespace Endpoint.Core.Models.Syntax.Fields;

public class FieldsSyntaxGenerationStrategy : SyntaxGenerationStrategyBase<IEnumerable<FieldModel>>
{
    private readonly ILogger<FieldsSyntaxGenerationStrategy> _logger;
    public FieldsSyntaxGenerationStrategy(
        IServiceProvider serviceProvider,
        ILogger<FieldsSyntaxGenerationStrategy> logger) 
        : base(serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override string Create(ISyntaxGenerationStrategyFactory syntaxGenerationStrategyFactory, IEnumerable<FieldModel> model, dynamic configuration = null)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        foreach(var field in model)
        {
            builder.AppendLine(Create(syntaxGenerationStrategyFactory, field, configuration));

            builder.AppendLine();
        }

        return builder.ToString();
    }

    private string Create(ISyntaxGenerationStrategyFactory syntaxGenerationStrategyFactory, FieldModel model, dynamic configuration = null)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();


        return builder.ToString();
    }
}
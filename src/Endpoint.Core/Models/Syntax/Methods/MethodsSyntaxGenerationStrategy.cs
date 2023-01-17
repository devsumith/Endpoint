using Endpoint.Core.Abstractions;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Endpoint.Core.Models.Syntax.Methods;

public class MethodsSyntaxGenerationStrategy : SyntaxGenerationStrategyBase<List<MethodModel>>
{
    private readonly ILogger<MethodsSyntaxGenerationStrategy> _logger;
    public MethodsSyntaxGenerationStrategy(
        ILogger<MethodsSyntaxGenerationStrategy> logger,
        IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override string Create(ISyntaxGenerationStrategyFactory syntaxGenerationStrategyFactory, List<MethodModel> model, dynamic context = null)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        foreach (var method in model)
        {
            builder.AppendLine(syntaxGenerationStrategyFactory.CreateFor(method, context));

            if (method != model.Last())
            {
                builder.AppendLine();
            }
        }

        return builder.ToString();
    }
}

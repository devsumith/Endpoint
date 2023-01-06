using Endpoint.Core.Abstractions;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Endpoint.Core.Models.Syntax.Fields;

internal class FieldSyntaxGenerationStrategy : SyntaxGenerationStrategyBase<FieldModel>
{
    private readonly ILogger<FieldSyntaxGenerationStrategy> _logger;
    public FieldSyntaxGenerationStrategy(
        IServiceProvider serviceProvider,
        ILogger<FieldSyntaxGenerationStrategy> logger) 
        : base(serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override string Create(ISyntaxGenerationStrategyFactory syntaxGenerationStrategyFactory, FieldModel model, dynamic configuration = null)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();


        return builder.ToString();
    }
}
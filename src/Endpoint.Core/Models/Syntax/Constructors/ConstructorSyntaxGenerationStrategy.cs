using Endpoint.Core.Abstractions;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Endpoint.Core.Models.Syntax.Constructors;

internal class ConstructorSyntaxGenerationStrategy : SyntaxGenerationStrategyBase<ConstructorModel>
{
    private readonly ILogger<ConstructorSyntaxGenerationStrategy> _logger;
    public ConstructorSyntaxGenerationStrategy(
        IServiceProvider serviceProvider,
        ILogger<ConstructorSyntaxGenerationStrategy> logger) 
        : base(serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override string Create(ISyntaxGenerationStrategyFactory syntaxGenerationStrategyFactory, ConstructorModel model, dynamic configuration = null)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        builder.AppendLine($"");

        return builder.ToString();
    }
}
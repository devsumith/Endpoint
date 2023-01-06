using Endpoint.Core.Abstractions;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Endpoint.Core.Models.Syntax.Methods;

internal class MethodSyntaxGenerationStrategy : SyntaxGenerationStrategyBase<MethodModel>
{
    private readonly ILogger<MethodSyntaxGenerationStrategy> _logger;
    public MethodSyntaxGenerationStrategy(
        IServiceProvider serviceProvider,
        ILogger<MethodSyntaxGenerationStrategy> logger) 
        : base(serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override string Create(ISyntaxGenerationStrategyFactory syntaxGenerationStrategyFactory, MethodModel model, dynamic configuration = null)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();


        return builder.ToString();
    }
}
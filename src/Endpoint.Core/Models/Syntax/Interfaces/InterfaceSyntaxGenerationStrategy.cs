using Endpoint.Core.Abstractions;
using Microsoft.Extensions.Logging;
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

    public override string Create(ISyntaxGenerationStrategyFactory syntaxGenerationStrategyFactory, InterfaceModel model, dynamic configuration = null)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();


        return builder.ToString();
    }
}
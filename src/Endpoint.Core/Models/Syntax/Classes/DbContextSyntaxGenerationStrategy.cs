using Endpoint.Core.Abstractions;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Endpoint.Core.Models.Syntax.Classes;

public class DbContextSyntaxGenerationStrategy : SyntaxGenerationStrategyBase<DbContextModel>
{
    private readonly ILogger<DbContextSyntaxGenerationStrategy> _logger;
    public DbContextSyntaxGenerationStrategy(
        IServiceProvider serviceProvider,
        ILogger<DbContextSyntaxGenerationStrategy> logger) 
        : base(serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override string Create(ISyntaxGenerationStrategyFactory syntaxGenerationStrategyFactory, DbContextModel model, dynamic configuration = null)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();


        return builder.ToString();
    }
}
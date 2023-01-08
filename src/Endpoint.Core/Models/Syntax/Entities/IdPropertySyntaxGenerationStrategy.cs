using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Syntax.Properties;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Endpoint.Core.Models.Syntax.Entities;

public class IdPropertySyntaxGenerationStrategy : SyntaxGenerationStrategyBase<PropertyModel>
{
    private readonly ILogger<IdPropertySyntaxGenerationStrategy> _logger;
    private readonly ISyntaxService _syntaxService;

    public IdPropertySyntaxGenerationStrategy(
        IServiceProvider serviceProvider,
        ISyntaxService syntaxService,
        ILogger<IdPropertySyntaxGenerationStrategy> logger) 
        : base(serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _syntaxService = syntaxService ?? throw new ArgumentNullException(nameof(syntaxService));
    }

    public override int Priority => 1;

    public override bool CanHandle(object model, dynamic configuration = null)
        => model is PropertyModel propertyModel && propertyModel.Id;

    public override string Create(ISyntaxGenerationStrategyFactory syntaxGenerationStrategyFactory, PropertyModel model, dynamic configuration = null)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();


        return builder.ToString();
    }
}
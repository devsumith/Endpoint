using Endpoint.Core.Abstractions;

namespace Endpoint.Core.Models.Syntax.Controllers;

internal class ControllerSyntaxGenerationStrategy : SyntaxGenerationStrategyBase<ControllerModel>
{
    public ControllerSyntaxGenerationStrategy(IServiceProvider serviceProvider) 
        : base(serviceProvider) { }

    public override string Create(ISyntaxGenerationStrategyFactory syntaxGenerationStrategyFactory, ControllerModel model, dynamic context = null)
    {
        throw new NotImplementedException();
    }
}

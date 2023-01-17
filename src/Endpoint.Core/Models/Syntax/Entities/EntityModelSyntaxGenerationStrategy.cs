using Endpoint.Core.Abstractions;

namespace Endpoint.Core.Models.Syntax.Entities;

public class EntityModelSyntaxGenerationStrategy : SyntaxGenerationStrategyBase<EntityModel>
{
    public EntityModelSyntaxGenerationStrategy(IServiceProvider serviceProvider)
        : base(serviceProvider)
    {

    }

    public override string Create(ISyntaxGenerationStrategyFactory syntaxGenerationStrategyFactory, EntityModel model, dynamic context = null)
    {
        throw new NotImplementedException();
    }
}

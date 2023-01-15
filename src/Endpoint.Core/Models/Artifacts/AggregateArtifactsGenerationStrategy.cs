using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Syntax.Entities.Aggregate;

namespace Endpoint.Core.Models.Artifacts;

public class AggregateArtifactsGenerationStrategy : ArtifactGenerationStrategyBase<AggregateModel>
{
    public AggregateArtifactsGenerationStrategy(IServiceProvider serviceProvider) 
        :base(serviceProvider) { }

    public override void Create(IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory, AggregateModel model, dynamic configuration = null)
    {
        throw new NotImplementedException();
    }
}

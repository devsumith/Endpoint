using Endpoint.Core.Abstractions;

namespace Endpoint.Core.Models.WebArtifacts;

public class AngularProjectGenerationStrategy : WebGenerationStrategyStrategyBase<AngularProjectModel>
{
    public AngularProjectGenerationStrategy(IServiceProvider serviceProvider)
        :base(serviceProvider)
    { }

    public override string Create(IWebGenerationStrategyFactory webGenerationStrategyFactory, AngularProjectModel model, dynamic configuration = null)
    {
        throw new NotImplementedException();
    }
}


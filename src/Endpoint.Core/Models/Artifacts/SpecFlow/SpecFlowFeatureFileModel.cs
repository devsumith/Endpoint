using Endpoint.Core.Models.Artifacts.Files;
using Endpoint.Core.Models.Syntax.SpecFlow;

namespace Endpoint.Core.Models.Artifacts.SpecFlow;

public class SpecFlowFeatureFileModel : ObjectFileModel<SpecFlowFeatureModel>
{
    public SpecFlowFeatureFileModel(SpecFlowFeatureModel model, string directory)
        : base(model, model.Name, directory, "feature")
    {

    }
}

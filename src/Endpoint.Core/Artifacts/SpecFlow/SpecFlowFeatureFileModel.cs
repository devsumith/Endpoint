// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Syntax.SpecFlow;

namespace Endpoint.Core.Artifacts.SpecFlow;

public class SpecFlowFeatureFileModel : ObjectFileModel<SpecFlowFeatureModel>
{
    public SpecFlowFeatureFileModel(SpecFlowFeatureModel model, string directory)
        : base(model, model.Name, directory, "feature")
    {

    }
}


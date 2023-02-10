// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Models.Artifacts.Files;

namespace Endpoint.Core.Models.Syntax.SpecFlow;

public class SpecFlowFeatureModel {

    public SpecFlowFeatureModel(string name)
    {
        Name = name;
    }
    public string Name { get; set; }
}


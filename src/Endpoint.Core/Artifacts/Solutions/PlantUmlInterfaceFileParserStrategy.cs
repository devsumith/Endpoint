// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Syntax.Interfaces;

namespace Endpoint.Core.Artifacts.Solutions;

public class PlantUmlInterfaceFileParserStrategy : PlantUmlParserStrategyBase<ObjectFileModel<InterfaceModel>>
{
    public PlantUmlInterfaceFileParserStrategy(IServiceProvider serviceProvider)
        : base(serviceProvider)

    { }

    public override bool CanHandle(string plantUml) => plantUml.StartsWith("class");

    protected override ObjectFileModel<InterfaceModel> Create(IPlantUmlParserStrategyFactory factory, string plantUml, dynamic context = null)
    {
        throw new NotImplementedException();
    }
}


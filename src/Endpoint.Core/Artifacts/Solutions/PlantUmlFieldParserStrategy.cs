// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Syntax.Properties;

namespace Endpoint.Core.Artifacts.Solutions;

public class PlantUmlFieldParserStrategy : PlantUmlParserStrategyBase<PropertyModel>
{
    public PlantUmlFieldParserStrategy(IServiceProvider serviceProvider)
        :base(serviceProvider)
    { }

    public override bool CanHandle(string plantUml) => plantUml.StartsWith("#");

    protected override PropertyModel Create(IPlantUmlParserStrategyFactory factory, string plantUml, dynamic context = null)
    {
        throw new NotImplementedException();
    }
}


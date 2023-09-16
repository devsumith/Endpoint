// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Syntax.Properties;
using System.Collections.Generic;

namespace Endpoint.Core.Syntax.Units.Factories;

public interface ISyntaxUnitFactory
{
    Task<AggregateModel> CreateAsync(string name, List<PropertyModel> properties);
    Task CreateAsync();
}


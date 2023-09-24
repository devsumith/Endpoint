// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using Endpoint.Core.Syntax.Properties;

namespace Endpoint.Core.Syntax.Documents.Factories;

public interface ISyntaxUnitFactory
{
    Task<AggregateModel> CreateAsync(string name, List<PropertyModel> properties);

    Task CreateAsync();
}

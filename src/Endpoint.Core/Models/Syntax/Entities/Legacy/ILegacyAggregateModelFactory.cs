// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.Models.Syntax.Entities.Legacy;

public interface ILegacyAggregateModelFactory
{
    LegacyAggregateModel Create(string resource, string properties);
}

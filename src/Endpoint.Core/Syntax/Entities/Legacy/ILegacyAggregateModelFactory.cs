// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.Syntax.Entities.Legacy;

public interface ILegacyAggregatesFactory
{
    LegacyAggregatesModel Create(string resource, string properties);
}

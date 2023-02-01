// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.Models.Syntax.Entities;

public interface IEntityModelFactory
{
    EntityModel Create(string name, string properties);
}

// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Models.Syntax.Attributes;

namespace Endpoint.Core.Strategies.CSharp.Attributes;

public interface IAttributeGenerationStrategy
{
    public bool CanHandle(AttributeModel model);
    public string Create(AttributeModel model);
}


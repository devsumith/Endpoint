// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Syntax.Attributes;

namespace Endpoint.Core.Strategies.CSharp.Attributes;

public class ConsumesAttributeGenerationStrategy : IAttributeGenerationStrategy
{
    public bool CanHandle(AttributeModel model) => model.Type == AttributeType.Consumes;

    public string Create(AttributeModel model) => "[Consumes(MediaTypeNames.Application.Json)]";
}


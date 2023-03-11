// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.Models.Syntax;

public class UsingDirectiveModel
{
    public UsingDirectiveModel(string name)
    {
        Name = name;
    }

    public UsingDirectiveModel()
    {
        
    }

    public string Name { get; init; }
}

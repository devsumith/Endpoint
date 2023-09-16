// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.Syntax;

public class UsingModel
{
    public UsingModel(string name)
    {
        Name = name;
    }

    public UsingModel()
    {
    }

    public string Name { get; init; }
}

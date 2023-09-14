// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.Syntax;

public class UsingAsModel
{
    public UsingAsModel(string name, string @as)
    {
        Name = name;
        Alias = @as;
    }

    public string Name { get; set; }

    public string Alias { get; set; }
}

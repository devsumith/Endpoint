// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.Syntax;

public class SyntaxModel
{
    public SyntaxModel(IdPropertyType idPropertyType, IdPropertyFormat idPropertyFormat, string dbContextName)
    {
        IdPropertyType = idPropertyType;
        IdPropertyFormat = idPropertyFormat;
        DbContextName = dbContextName;
    }

    public IdPropertyType IdPropertyType { get; set; }
    public IdPropertyFormat IdPropertyFormat { get; init; }
    public string DbContextName { get; set; }
}


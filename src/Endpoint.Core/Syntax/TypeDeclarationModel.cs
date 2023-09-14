// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Syntax.Properties;
using System.Collections.Generic;

namespace Endpoint.Core.Syntax;

public class TypeDeclarationModel: SyntaxModel
{
    public TypeDeclarationModel(string name)
    {
        Name = name;
        Properties = new List<PropertyModel>();
        UsingDirectives = new List<UsingModel>();
        UsingAsDirectives = new List<UsingAsDirectiveModel>();
    }

    public string Name { get; set; }
    public List<PropertyModel> Properties { get; set; }
    public List<UsingModel> UsingDirectives { get; set; }
    public List<UsingAsDirectiveModel> UsingAsDirectives { get; set; }
}


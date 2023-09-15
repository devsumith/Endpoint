// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Endpoint.Core.Syntax.Namespaces;

public class NamespaceModel : SyntaxModel
{
    public NamespaceModel()
    {

    }
    public NamespaceModel(string name, List<SyntaxModel> syntaxModels)
    {
        Name = name;
        SyntaxModels = syntaxModels;
    }

    public string Name { get; set; }
    public List<SyntaxModel> SyntaxModels { get; set; }
}


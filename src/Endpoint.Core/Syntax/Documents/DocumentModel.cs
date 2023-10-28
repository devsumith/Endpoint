// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Endpoint.Core.Syntax.Documents;

public class DocumentModel : SyntaxModel
{
    public DocumentModel()
    {
        Code = new List<SyntaxModel>();
    }

    public DocumentModel(string name, string @namespace)
    {
        Name = name;
        Code = new List<SyntaxModel>();
        Namespace = @namespace;
    }

    public string Name { get; set; }

    public string RootNamespace { get; set; }

    public string Namespace { get; set; }

    public List<SyntaxModel> Code { get; set; }

    public override IEnumerable<SyntaxModel> GetChildren()
    {
        foreach (var model in Code)
        {
            yield return model;
        }
    }
}

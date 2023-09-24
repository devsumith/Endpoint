// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Endpoint.Core.Syntax.Documents;

public class DocumentModel : SyntaxModel
{
    public DocumentModel()
    {
    }

    public DocumentModel(string name)
    {
        Name = name;
        Code = new List<SyntaxModel>();
    }

    public string Name { get; set; }

    public List<SyntaxModel> Code { get; set; }
}

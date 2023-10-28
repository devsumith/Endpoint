// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Endpoint.Core.Syntax;

public class SyntaxModel
{
    public SyntaxModel()
    {
        Usings = new List<UsingModel>();
    }

    public SyntaxModel Parent { get; set; }

    public List<UsingModel> Usings { get; set; }

    public virtual IEnumerable<SyntaxModel> GetChildren()
    {
        yield break;
    }

    public virtual List<SyntaxModel> GetDescendants(SyntaxModel syntax = null, List<SyntaxModel> children = null)
    {
        syntax ??= this;

        children ??= new ();

        children.Add(syntax);

        foreach (var child in syntax.GetChildren())
        {
            if (child != null)
            {
                GetDescendants(child, children);
            }
        }

        return children;
    }
}

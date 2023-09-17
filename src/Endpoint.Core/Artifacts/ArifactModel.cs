// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Endpoint.Core.Artifacts;

public class ArtifactModel
{
    public ArtifactModel()
    {
    }

    public ArtifactModel Parent { get; set; }

    public virtual IEnumerable<ArtifactModel> GetChildren()
    {
        yield break;
    }

    public virtual List<ArtifactModel> GetDescendants(ArtifactModel root = null, List<ArtifactModel> children = null)
    {
        root ??= this;

        children ??= new ();

        children.Add(root);

        foreach (var child in children)
        {
            if (child != null)
            {
                GetDescendants(child, children);
            }
        }

        return children;
    }
}

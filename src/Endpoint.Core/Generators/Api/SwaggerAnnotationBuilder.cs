// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Endpoint.Core.Builders;

public class SwaggerAnnotationBuilder
{
    private int _indent = 0;
    private string _summary;
    private string _description;
    public SwaggerAnnotationBuilder(string summary, string description, int indent = 0)
    {
        _summary = summary;
        _description = description;
        _indent = indent;
    }

    public string[] Build()
    {
        var results = new List<string>
        {
            "[SwaggerOperation(".Indent(_indent),
            $"Summary = \"{_summary}\",".Indent(_indent + 1),

            $"Description = @\"{_description}\"".Indent(_indent + 1),

            ")]".Indent(_indent)
        };

        return results.ToArray();
    }
}


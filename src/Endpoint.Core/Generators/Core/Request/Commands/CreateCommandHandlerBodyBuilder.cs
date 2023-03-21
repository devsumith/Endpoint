// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core;
using Endpoint.Core.Models.Syntax;
using Endpoint.Core.Models.Syntax.Entities.Legacy;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core.Builders.Core
{
    public class CreateCommandHandlerBodyBuilder
    {
        public static string[] Build(LegacyAggregatesModel aggregateRoot)
        {
            var aggregateName = aggregateRoot.Name;

            var result = new List<string>() {
                $"var {((SyntaxToken)aggregateName).CamelCase} = new {((SyntaxToken)aggregateName).PascalCase}();",
                "",
                $"_context.{((SyntaxToken)aggregateName).PascalCasePlural}.Add({((SyntaxToken)aggregateName).CamelCase});",
                "",

                };

            foreach (var property in aggregateRoot.Properties.Where(x => x.Id == false))
            {
                result.Add($"{((SyntaxToken)aggregateName).CamelCase}.{((SyntaxToken)property.Name).PascalCase} = request.{((SyntaxToken)aggregateName).PascalCase}.{((SyntaxToken)property.Name).PascalCase};");
            }

            result = result.Concat(new List<string>() {
                "",
                "await _context.SaveChangesAsync(cancellationToken);",
                "",
                "return new ()",
                "{",
                $"{((SyntaxToken)aggregateName).PascalCase} = {((SyntaxToken)aggregateName).CamelCase}.ToDto()".Indent(1),
                "};"
                }).ToList();

            return result.ToArray();
        }
    }
}


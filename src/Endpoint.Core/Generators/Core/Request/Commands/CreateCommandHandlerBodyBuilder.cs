using Endpoint.Core;
using Endpoint.Core.Models.Syntax.Entities;
using Endpoint.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core.Builders.Core
{
    public class CreateCommandHandlerBodyBuilder
    {
        public static string[] Build(AggregateRootModel aggregateRoot)
        {
            var aggregateName = aggregateRoot.Name;

            var result = new List<string>() {
                $"var {((Token)aggregateName).CamelCase} = new {((Token)aggregateName).PascalCase}();",
                "",
                $"_context.{((Token)aggregateName).PascalCasePlural}.Add({((Token)aggregateName).CamelCase});",
                "",

                };

            foreach (var property in aggregateRoot.Properties.Where(x => x.Key == false))
            {
                result.Add($"{((Token)aggregateName).CamelCase}.{((Token)property.Name).PascalCase} = request.{((Token)aggregateName).PascalCase}.{((Token)property.Name).PascalCase};");
            }

            result = result.Concat(new List<string>() {
                "",
                "await _context.SaveChangesAsync(cancellationToken);",
                "",
                "return new ()",
                "{",
                $"{((Token)aggregateName).PascalCase} = {((Token)aggregateName).CamelCase}.ToDto()".Indent(1),
                "};"
                }).ToList();

            return result.ToArray();
        }
    }
}

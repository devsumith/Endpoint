using Endpoint.Core.Builders.Common;
using Endpoint.Core.Models;
using Endpoint.Core.ValueObjects;
using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core.Builders.Core
{
    public class UpdateCommandHandlerBodyBuilder
    {
        public static string[] Build(Settings settings, AggregateRoot aggregateRoot)
        {
            var aggregateName = aggregateRoot.Name;

            var result = new List<string>() {
                $"var {((Token)aggregateName).CamelCase} = await _context.{((Token)aggregateName).PascalCasePlural}.SingleAsync(x => x.{IdPropertyNameBuilder.Build(settings,aggregateName)} == request.{((Token)aggregateName).PascalCase}.{IdPropertyNameBuilder.Build(settings,aggregateName)});",
                "",

                };

            foreach (var property in aggregateRoot.Properties.Where(x => x.Key == false))
            {
                result.Add($"{((Token)aggregateName).CamelCase}.{((Token)property.Name).PascalCase} = request.{((Token)aggregateName).PascalCase}.{((Token)property.Name).PascalCase};");
            }

            return result.Concat(new List<string>() {
                "",
                "await _context.SaveChangesAsync(cancellationToken);",
                "",
                "return new ()",
                "{",
                $"{((Token)aggregateName).PascalCase} = {((Token)aggregateName).CamelCase}.ToDto()".Indent(1),
                "};"
                }).ToArray();

        }
    }
}

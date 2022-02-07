using Endpoint.SharedKernal;
using Endpoint.SharedKernal.Models;
using Endpoint.SharedKernal.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Endpoint.Application.Core.Builders.Core
{
    public class UpdateCommandHandlerBodyBuilder
    {
        public static string[] Build(AggregateRoot aggregateRoot)
        {
            var aggregateName = aggregateRoot.Name;

            var result = new List<string>() {
                $"var {((Token)aggregateName).CamelCase} = await _context.{((Token)aggregateName).PascalCasePlural}.SingleAsync(x => x.{((Token)aggregateName).PascalCase}Id == request.{((Token)aggregateName).PascalCase}.{((Token)aggregateName).PascalCase}Id);",
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

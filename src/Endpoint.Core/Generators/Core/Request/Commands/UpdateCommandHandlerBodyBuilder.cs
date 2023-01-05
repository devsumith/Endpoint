using Endpoint.Core.Builders.Common;
using Endpoint.Core.Models.Options;
using Endpoint.Core.Models.Syntax;
using Endpoint.Core.Models.Syntax.Entities;
using Endpoint.Core.ValueObjects;
using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core.Builders.Core
{
    public class UpdateCommandHandlerBodyBuilder
    {
        public static string[] Build(SettingsModel settings, AggregateRootModel aggregateRoot)
        {
            var aggregateName = aggregateRoot.Name;

            var result = settings.IdDotNetType == IdDotNetType.Int
                ? new List<string>() { $"var {((Token)aggregateName).CamelCase} = await _context.{((Token)aggregateName).PascalCasePlural}.SingleAsync(x => x.{IdPropertyNameBuilder.Build(settings, aggregateName)} == request.{((Token)aggregateName).PascalCase}.{IdPropertyNameBuilder.Build(settings, aggregateName)});", "" }
                : new List<string>() { $"var {((Token)aggregateName).CamelCase} = await _context.{((Token)aggregateName).PascalCasePlural}.SingleAsync(x => x.{IdPropertyNameBuilder.Build(settings, aggregateName)} == new {((Token)aggregateName).PascalCase}Id(request.{((Token)aggregateName).PascalCase}.{IdPropertyNameBuilder.Build(settings, aggregateName)}.Value));", "" };


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

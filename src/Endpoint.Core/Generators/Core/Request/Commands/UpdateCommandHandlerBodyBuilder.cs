using Endpoint.Core.Builders.Common;
using Endpoint.Core.Models.Options;
using Endpoint.Core.Models.Syntax;
using Endpoint.Core.Models.Syntax.Entities.Legacy;
using Endpoint.Core.ValueObjects;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Endpoint.Core.Builders.Core
{
    public class UpdateCommandHandlerBodyBuilder
    {
        public static string[] Build(SettingsModel settings, LegacyAggregateModel aggregateRoot)
        {
            var entityName = aggregateRoot.Name;

            var entityNamePascalCasePlural = ((Token)entityName).PascalCasePlural;

            var entityNameCamelCase = ((Token)entityName).CamelCase;

            var builder = new StringBuilder();

            builder.AppendLine($"var {entityNameCamelCase} = await _context.{entityNamePascalCasePlural}.SingleAsync(x => x.{entityName}Id == new {entityName}Id(request.{entityName}.{entityName}Id.Value));");

            builder.AppendLine("");

            foreach (var property in aggregateRoot.Properties.Where(x => x.Id == false))
            {
                builder.AppendLine($"{entityNameCamelCase}.{property.Name} = request.{entityName}.{property.Name};");
            }

            builder.AppendLine("");

            builder.AppendLine("await _context.SaveChangesAsync(cancellationToken);");

            builder.AppendLine("");

            builder.AppendLine("return new ()");

            builder.AppendLine("{");

            builder.AppendLine($"{entityName} = {entityNameCamelCase}.ToDto()".Indent(1));

            builder.AppendLine("}");

            return builder.ToString().Split(Environment.NewLine);


        }
    }
}

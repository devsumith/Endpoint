using Endpoint.Core.Models.Syntax.Classes;
using Endpoint.Core.ValueObjects;
using System.Collections.Generic;

namespace Endpoint.Core.Strategies.Infrastructure
{
    public interface IDbContextGenerationStrategy
    {
        string Create(DbContextModel model);
    }
    public class DbContextGenerationStrategy: IDbContextGenerationStrategy
    {
        public string Create(DbContextModel model)
        {
            var content = new List<string>
            {
                $"public class {((Token)model.Name).PascalCase} : DbContext",

                "{",

                $"public {((Token)model.Name).PascalCase}(DbContextOptions<{((Token)model.Name).PascalCase}> options)".Indent(1),

                ": base(options) { }".Indent(2),

                ""
            };

            foreach (var aggregateRoot in model.Entities)
            {
                content.Add($"public DbSet<{((Token)aggregateRoot.Name).PascalCase}> {((Token)aggregateRoot.Name).PascalCasePlural} => Set<{((Token)aggregateRoot.Name).PascalCase}>();".Indent(1));
            }

            content.Add("}");

            return string.Join(Environment.NewLine, content);
        }
    }
}

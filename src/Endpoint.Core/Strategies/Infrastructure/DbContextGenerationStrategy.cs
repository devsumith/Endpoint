using Endpoint.Core.Models;
using Endpoint.Core.ValueObjects;
using System.Collections.Generic;

namespace Endpoint.Core.Strategies.Infrastructure
{
    public interface IDbContextGenerationStrategy
    {
        string[] Create(DbContextModel model);
    }
    public class DbContextGenerationStrategy: IDbContextGenerationStrategy
    {
        public string[] Create(DbContextModel model)
        {
            var content = new List<string>();

            content.Add($"class {((Token)model.Name).PascalCase} : DbContext");

            content.Add("{");

            content.Add($"public {((Token)model.Name).PascalCase}(DbContextOptions<{((Token)model.Name).PascalCase}> options)".Indent(1));

            content.Add(": base(options) { }".Indent(2));

            content.Add("");

            foreach(var aggregateRoot in model.AggregateRoots)
            {
                content.Add($"public DbSet<{((Token)aggregateRoot.Name).PascalCase}> {((Token)aggregateRoot.Name).PascalCasePlural} => Set<{((Token)aggregateRoot.Name).PascalCase}>();".Indent(1));
            }

            content.Add("}");

            return content.ToArray();
        }
    }
}

// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Models.Syntax;
using Endpoint.Core.Models.Syntax.Classes;
using System.Collections.Generic;

namespace Endpoint.Core.Strategies.Infrastructure
{
    public interface IDbContextGenerationStrategy
    {
        string Create(DbContextModel model);
    }
    public class DbContextGenerationStrategy : IDbContextGenerationStrategy
    {
        public string Create(DbContextModel model)
        {
            var content = new List<string>
            {
                $"public class {((SyntaxToken)model.Name).PascalCase} : DbContext",

                "{",

                $"public {((SyntaxToken)model.Name).PascalCase}(DbContextOptions<{((SyntaxToken)model.Name).PascalCase}> options)".Indent(1),

                ": base(options) { }".Indent(2),

                ""
            };

            foreach (var aggregateRoot in model.Entities)
            {
                content.Add($"public DbSet<{((SyntaxToken)aggregateRoot.Name).PascalCase}> {((SyntaxToken)aggregateRoot.Name).PascalCasePlural} => Set<{((SyntaxToken)aggregateRoot.Name).PascalCase}>();".Indent(1));
            }

            content.Add("}");

            return string.Join(Environment.NewLine, content);
        }
    }
}


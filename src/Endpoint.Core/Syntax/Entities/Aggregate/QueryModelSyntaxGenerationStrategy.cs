// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using System.Text;

namespace Endpoint.Core.Syntax.Entities.Aggregate;

public class QueryModelSyntaxGenerationStrategy : SyntaxGenerationStrategyBase<QueryModel>
{
    public QueryModelSyntaxGenerationStrategy(IServiceProvider serviceProvider)
        : base(serviceProvider) { }

    public override async Task<string> CreateAsync(ISyntaxGenerator syntaxGenerator, QueryModel model, dynamic context = null)
    {
        var builder = new StringBuilder();

        builder.AppendLine(await syntaxGenerator.CreateAsync(model.Request));

        builder.AppendLine("");

        builder.AppendLine(await syntaxGenerator.CreateAsync(model.Response));

        builder.AppendLine("");

        builder.AppendLine(await syntaxGenerator.CreateAsync(model.RequestHandler, context));

        return builder.ToString();
    }
}


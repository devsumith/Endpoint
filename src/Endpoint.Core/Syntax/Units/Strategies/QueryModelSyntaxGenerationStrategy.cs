// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Text;
using Endpoint.Core.Syntax.Units;

namespace Endpoint.Core.Syntax.Units.Strategies;

public class QueryModelSyntaxGenerationStrategy : GenericSyntaxGenerationStrategy<QueryModel>
{
    public QueryModelSyntaxGenerationStrategy(IServiceProvider serviceProvider)
    {

    }



    public override async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, QueryModel model)
    {
        var builder = new StringBuilder();

        builder.AppendLine(await syntaxGenerator.GenerateAsync(model.Request));

        builder.AppendLine("");

        builder.AppendLine(await syntaxGenerator.GenerateAsync(model.Response));

        builder.AppendLine("");

        builder.AppendLine(await syntaxGenerator.GenerateAsync(model.RequestHandler));

        return builder.ToString();
    }
}


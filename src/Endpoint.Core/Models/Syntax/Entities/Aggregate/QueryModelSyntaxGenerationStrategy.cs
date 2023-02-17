// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using System.Text;

namespace Endpoint.Core.Models.Syntax.Entities.Aggregate;

public class QueryModelSyntaxGenerationStrategy : SyntaxGenerationStrategyBase<QueryModel>
{
    public QueryModelSyntaxGenerationStrategy(IServiceProvider serviceProvider)
        : base(serviceProvider) { }

    public override string Create(ISyntaxGenerationStrategyFactory syntaxGenerationStrategyFactory, QueryModel model, dynamic context = null)
    {
        var builder = new StringBuilder();

        builder.AppendLine(syntaxGenerationStrategyFactory.CreateFor(model.Request));

        builder.AppendLine("");

        builder.AppendLine(syntaxGenerationStrategyFactory.CreateFor(model.Response));

        builder.AppendLine("");

        builder.AppendLine(syntaxGenerationStrategyFactory.CreateFor(model.RequestHandler, context));

        return builder.ToString();
    }
}


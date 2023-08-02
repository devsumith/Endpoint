// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using System.Text;

namespace Endpoint.Core.Models.Syntax.Entities.Aggregate;

public class CommandModelSyntaxGenerationStrategy : SyntaxGenerationStrategyBase<CommandModel>
{
    public CommandModelSyntaxGenerationStrategy(IServiceProvider serviceProvider)
        : base(serviceProvider) { }

    public override string Create(ISyntaxGenerator syntaxGenerator, CommandModel model, dynamic context = null)
    {
        var builder = new StringBuilder();

        builder.AppendLine(syntaxGenerator.CreateFor(model.RequestValidator, context));

        builder.AppendLine("");

        builder.AppendLine(syntaxGenerator.CreateFor(model.Request, context));

        builder.AppendLine("");

        builder.AppendLine(syntaxGenerator.CreateFor(model.Response, context));

        builder.AppendLine("");

        builder.AppendLine(syntaxGenerator.CreateFor(model.RequestHandler, context));

        return builder.ToString();
    }
}

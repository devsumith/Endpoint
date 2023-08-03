// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using System.Text;

namespace Endpoint.Core.Syntax.Entities.Aggregate;

public class CommandModelSyntaxGenerationStrategy : SyntaxGenerationStrategyBase<CommandModel>
{
    public CommandModelSyntaxGenerationStrategy(IServiceProvider serviceProvider)
        : base(serviceProvider) { }

    public override async Task<string> CreateAsync(ISyntaxGenerator syntaxGenerator, CommandModel model, dynamic context = null)
    {
        var builder = new StringBuilder();

        builder.AppendLine(await syntaxGenerator.CreateAsync(model.RequestValidator, context));

        builder.AppendLine("");

        builder.AppendLine(await syntaxGenerator.CreateAsync(model.Request, context));

        builder.AppendLine("");

        builder.AppendLine(await syntaxGenerator.CreateAsync(model.Response, context));

        builder.AppendLine("");

        builder.AppendLine(await syntaxGenerator.CreateAsync(model.RequestHandler, context));

        return builder.ToString();
    }
}

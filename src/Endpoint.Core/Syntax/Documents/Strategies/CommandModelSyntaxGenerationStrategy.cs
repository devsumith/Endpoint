// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Text;

namespace Endpoint.Core.Syntax.Documents.Strategies;

public class CommandModelSyntaxGenerationStrategy : GenericSyntaxGenerationStrategy<CommandModel>
{
    public CommandModelSyntaxGenerationStrategy(IServiceProvider serviceProvider)
    {
    }

    public override async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, CommandModel model)
    {
        var builder = new StringBuilder();

        builder.AppendLine(await syntaxGenerator.GenerateAsync(model.RequestValidator));

        builder.AppendLine(string.Empty);

        builder.AppendLine(await syntaxGenerator.GenerateAsync(model.Request));

        builder.AppendLine(string.Empty);

        builder.AppendLine(await syntaxGenerator.GenerateAsync(model.Response));

        builder.AppendLine(string.Empty);

        builder.AppendLine(await syntaxGenerator.GenerateAsync(model.RequestHandler));

        return builder.ToString();
    }
}

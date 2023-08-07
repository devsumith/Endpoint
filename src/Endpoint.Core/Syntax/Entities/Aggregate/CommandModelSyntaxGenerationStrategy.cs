// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using System.Text;

namespace Endpoint.Core.Syntax.Entities.Aggregate;

public class CommandModelSyntaxGenerationStrategy : ISyntaxGenerationStrategy<CommandModel>
{
    public CommandModelSyntaxGenerationStrategy(IServiceProvider serviceProvider)
    { 
    
    }

    public int Priority => 0;

    public async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, CommandModel model, dynamic context = null)
    {
        var builder = new StringBuilder();

        builder.AppendLine(await syntaxGenerator.GenerateAsync(model.RequestValidator, context));

        builder.AppendLine("");

        builder.AppendLine(await syntaxGenerator.GenerateAsync(model.Request, context));

        builder.AppendLine("");

        builder.AppendLine(await syntaxGenerator.GenerateAsync(model.Response, context));

        builder.AppendLine("");

        builder.AppendLine(await syntaxGenerator.GenerateAsync(model.RequestHandler, context));

        return builder.ToString();
    }
}

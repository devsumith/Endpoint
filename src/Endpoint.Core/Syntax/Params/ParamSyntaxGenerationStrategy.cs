// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Extensions.Logging;
using System.Text;

namespace Endpoint.Core.Syntax.Params;

public class ParamSyntaxGenerationStrategy : GenericSyntaxGenerationStrategy<ParamModel>
{
    private readonly ILogger<ParamSyntaxGenerationStrategy> _logger;
    public ParamSyntaxGenerationStrategy(

        ILogger<ParamSyntaxGenerationStrategy> logger)

    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public int GetPriority() => 0;


    public override async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, ParamModel model)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        if (model.ExtensionMethodParam)
            builder.Append("this ");

        if (model.Attribute != null)
            builder.Append(await syntaxGenerator.GenerateAsync(model.Attribute));

        builder.Append($"{await syntaxGenerator.GenerateAsync(model.Type)} {model.Name}");

        return builder.ToString();
    }
}

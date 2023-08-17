// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Extensions.Logging;
using System.Text;

namespace Endpoint.Core.Syntax;

public class AccessModifierSyntaxGenerationStrategy : GenericSyntaxGenerationStrategy<AccessModifier>
{
    private readonly ILogger<AccessModifierSyntaxGenerationStrategy> _logger;
    public AccessModifierSyntaxGenerationStrategy(
        IServiceProvider serviceProvider,
        ILogger<AccessModifierSyntaxGenerationStrategy> logger)

    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }



    public override async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, AccessModifier model, dynamic context = null)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        builder.Append(model switch
        {
            AccessModifier.Public => "public",
            AccessModifier.Protected => "protected",
            AccessModifier.Private => "private",
            _ => "public"
        });

        return builder.ToString();
    }
}

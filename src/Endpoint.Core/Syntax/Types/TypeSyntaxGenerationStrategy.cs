// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Syntax.Types;

public class TypeSyntaxGenerationStrategy : GenericSyntaxGenerationStrategy<TypeModel>
{
    private readonly ILogger<TypeSyntaxGenerationStrategy> logger;

    public TypeSyntaxGenerationStrategy(

        ILogger<TypeSyntaxGenerationStrategy> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, TypeModel model)
    {
        logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        builder.Append(model.Name);

        if (model.GenericTypeParameters.Count > 0)
        {
            builder.Append('<');

            builder.AppendJoin(',', await Task.WhenAll(model.GenericTypeParameters.Select(async x => await syntaxGenerator.GenerateAsync(x))));

            builder.Append('>');
        }

        return builder.ToString();
    }
}

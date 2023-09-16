// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Syntax.Constructors;

public class ConstructorsSyntaxGenerationStrategy : GenericSyntaxGenerationStrategy<List<ConstructorModel>>
{
    private readonly ILogger<ConstructorsSyntaxGenerationStrategy> logger;

    public ConstructorsSyntaxGenerationStrategy(
        ILogger<ConstructorsSyntaxGenerationStrategy> logger,
        IServiceProvider serviceProvider)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, List<ConstructorModel> model)
    {
        logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        foreach (var ctor in model)
        {
            builder.AppendLine(await syntaxGenerator.GenerateAsync(ctor));

            if (ctor != model.Last())
            {
                builder.AppendLine();
            }
        }

        return builder.ToString();
    }
}

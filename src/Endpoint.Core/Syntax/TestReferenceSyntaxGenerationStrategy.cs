// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Text;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Syntax;

public class TestReferenceSyntaxGenerationStrategy : GenericSyntaxGenerationStrategy<TestReferenceModel>
{
    private readonly ILogger<TestReferenceSyntaxGenerationStrategy> logger;

    public TestReferenceSyntaxGenerationStrategy(

        ILogger<TestReferenceSyntaxGenerationStrategy> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, TestReferenceModel model)
    {
        logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        // TODO: Fix later
        // builder.AppendLine("// Test ID".Indent(2, 2));
        return builder.ToString();
    }
}

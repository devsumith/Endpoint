// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Endpoint.Core.Models.Syntax;

public class TestReferenceSyntaxGenerationStrategy : SyntaxGenerationStrategyBase<TestReferenceModel>
{
    private readonly ILogger<TestReferenceSyntaxGenerationStrategy> _logger;
    public TestReferenceSyntaxGenerationStrategy(
        IServiceProvider serviceProvider,
        ILogger<TestReferenceSyntaxGenerationStrategy> logger)
        : base(serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override string Create(ISyntaxGenerationStrategyFactory syntaxGenerationStrategyFactory, TestReferenceModel model, dynamic context = null)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        builder.AppendLine("// Test ID".Indent(2, 2));
        return builder.ToString();
    }
}

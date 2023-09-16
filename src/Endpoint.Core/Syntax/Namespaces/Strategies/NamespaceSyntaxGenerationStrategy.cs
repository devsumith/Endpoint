// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Text;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Syntax.Namespaces.Strategies;

public class NamespaceGenerationStrategy : GenericSyntaxGenerationStrategy<NamespaceModel>
{
    private readonly ILogger<NamespaceGenerationStrategy> logger;

    public NamespaceGenerationStrategy(ILogger<NamespaceGenerationStrategy> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task<string> GenerateAsync(ISyntaxGenerator generator, NamespaceModel model)
    {
        logger.LogInformation("Generating syntax. {name}", model.Name);

        StringBuilder sb = new StringBuilder();

        sb.AppendLine($"namespace {model.Name};");

        return sb.ToString();
    }
}
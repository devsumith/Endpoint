// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Extensions.Logging;
using System.Text;

namespace Endpoint.Core.Syntax.Documents.Strategies;

public class DocumentGenerationStrategy : GenericSyntaxGenerationStrategy<DocumentModel>
{
    private readonly ILogger<DocumentGenerationStrategy> _logger;

    public DocumentGenerationStrategy(ILogger<DocumentGenerationStrategy> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task<string> GenerateAsync(ISyntaxGenerator generator, DocumentModel model)
    {
        _logger.LogInformation("Generating syntax. {name}", model.Name);

        StringBuilder sb = new StringBuilder();

        return sb.ToString();
    }
}
// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Syntax.Documents.Strategies;

public class DocumentGenerationStrategy : GenericSyntaxGenerationStrategy<DocumentModel>
{
    private readonly ILogger<DocumentGenerationStrategy> logger;

    public DocumentGenerationStrategy(ILogger<DocumentGenerationStrategy> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task<string> GenerateAsync(ISyntaxGenerator generator, DocumentModel model)
    {
        logger.LogInformation("Generating syntax. {name}", model.Name);

        StringBuilder sb = new StringBuilder();

        foreach (var @using in model.GetDescendants().SelectMany(x => x.Usings).ToList())
        {
            sb.AppendLine($"using {@using.Name};");
        }

        sb.AppendLine();

        sb.AppendLine($"namespace {model.RootNamespace}.{model.Namespace};");

        sb.AppendLine();

        foreach (var item in model.Code)
        {
            string result = await generator.GenerateAsync(item);

            sb.AppendLine(result);

            sb.AppendLine();
        }

        return sb.ToString();
    }
}
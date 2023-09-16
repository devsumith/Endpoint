// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Syntax.Namespaces.Strategies;

public class NamespacePlantUmlParsingStrategy : BaseSyntaxParsingStrategy<NamespaceModel>
{
    private readonly ILogger<NamespacePlantUmlParsingStrategy> logger;
    private readonly IContext context;

    public NamespacePlantUmlParsingStrategy(IContext context, ILogger<NamespacePlantUmlParsingStrategy> logger)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task<NamespaceModel> ParseAsync(ISyntaxParser parser, string value)
    {
        logger.LogInformation("Parsing PlantUml for Namespace syntax. {typeName}", typeof(NamespaceModel).Name);

        return new NamespaceModel()
        {
            Name = value,
        };
    }
}
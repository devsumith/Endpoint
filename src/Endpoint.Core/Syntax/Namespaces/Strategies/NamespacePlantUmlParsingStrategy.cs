// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Extensions.Logging;
using System.Text;
using Endpoint.Core.Services;

namespace Endpoint.Core.Syntax.Namespaces.Strategies;

public class NamespacePlantUmlParsingStrategy : BaseSyntaxParsingStrategy<NamespaceModel>
{
    private readonly ILogger<NamespacePlantUmlParsingStrategy> _logger;
    private readonly IContext _context;

    public NamespacePlantUmlParsingStrategy(IContext context, ILogger<NamespacePlantUmlParsingStrategy> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task<NamespaceModel> ParseAsync(ISyntaxParser parser, string value)
    {
        _logger.LogInformation("Parsing PlantUml for syntax. {typeName}", typeof(NamespaceModel).Name);

        return new NamespaceModel();
    }
}
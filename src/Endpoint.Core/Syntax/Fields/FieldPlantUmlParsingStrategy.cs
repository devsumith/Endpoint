// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Extensions.Logging;
using System.Text;
using Endpoint.Core.Services;

namespace Endpoint.Core.Syntax.Fields;

public class FieldPlantUmlParsingStrategy : BaseSyntaxParsingStrategy<FieldModel>
{
    private readonly ILogger<FieldPlantUmlParsingStrategy> _logger;
    private readonly IContext _context;

    public FieldPlantUmlParsingStrategy(IContext context, ILogger<FieldPlantUmlParsingStrategy> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task<FieldModel> ParseAsync(ISyntaxParser parser, string value)
    {
        _logger.LogInformation("Parsing PlantUml for syntax. {typeName}", typeof(FieldModel).Name);

        throw new NotImplementedException();
    }
}
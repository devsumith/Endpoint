// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Syntax.Interfaces;

public class InterfacePlantUmlParsingStrategy : BaseSyntaxParsingStrategy<InterfaceModel>
{
    private readonly ILogger<InterfacePlantUmlParsingStrategy> logger;
    private readonly IContext context;

    public InterfacePlantUmlParsingStrategy(IContext context, ILogger<InterfacePlantUmlParsingStrategy> logger)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task<InterfaceModel> ParseAsync(ISyntaxParser parser, string value)
    {
        logger.LogInformation("Parsing PlantUml for syntax. {typeName}", typeof(InterfaceModel).Name);

        throw new NotImplementedException();
    }
}
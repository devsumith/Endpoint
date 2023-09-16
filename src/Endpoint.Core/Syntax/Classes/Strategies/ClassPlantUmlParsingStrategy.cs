// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Syntax.Classes.Strategies;

public class ClassPlantUmlParsingStrategy : BaseSyntaxParsingStrategy<ClassModel>
{
    private readonly ILogger<ClassPlantUmlParsingStrategy> logger;
    private readonly IContext context;

    public ClassPlantUmlParsingStrategy(IContext context, ILogger<ClassPlantUmlParsingStrategy> logger)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task<ClassModel> ParseAsync(ISyntaxParser parser, string value)
    {
        logger.LogInformation("Parsing PlantUml for syntax. {typeName}", typeof(ClassModel).Name);

        throw new NotImplementedException();
    }
}
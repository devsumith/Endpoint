// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Text;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Syntax.Classes.Strategies;

public class ControllerGenerationStrategy : GenericSyntaxGenerationStrategy<ClassModel>
{
    private readonly ILogger<ControllerGenerationStrategy> logger;

    public ControllerGenerationStrategy(ILogger<ControllerGenerationStrategy> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override int GetPriority() => int.MaxValue;

    public override Task<string> GenerateAsync(ISyntaxGenerator generator, object target)
    {
        return base.GenerateAsync(generator, target);
    }

    public override async Task<string> GenerateAsync(ISyntaxGenerator generator, ClassModel model)
    {
        logger.LogInformation("Generating syntax. {name}", model.Name);

        StringBuilder sb = new StringBuilder();

        return sb.ToString();
    }
}
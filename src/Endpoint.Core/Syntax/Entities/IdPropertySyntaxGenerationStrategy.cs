// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Text;
using Endpoint.Core.Syntax.Properties;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Syntax.Entities;

public class IdPropertySyntaxGenerationStrategy : GenericSyntaxGenerationStrategy<PropertyModel>
{
    private readonly ILogger<IdPropertySyntaxGenerationStrategy> logger;
    private readonly ISyntaxService syntaxService;

    public IdPropertySyntaxGenerationStrategy(

        ISyntaxService syntaxService,
        ILogger<IdPropertySyntaxGenerationStrategy> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.syntaxService = syntaxService ?? throw new ArgumentNullException(nameof(syntaxService));
    }

    public int GetPriority => 1;

    public bool CanHandle(object model)
        => model is PropertyModel propertyModel && propertyModel.Id;

    public override async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, PropertyModel model)
    {
        logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        return builder.ToString();
    }
}

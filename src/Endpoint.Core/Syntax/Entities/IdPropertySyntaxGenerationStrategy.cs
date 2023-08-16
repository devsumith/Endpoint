// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Syntax.Properties;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Endpoint.Core.Syntax.Entities;

public class IdPropertySyntaxGenerationStrategy : GenericSyntaxGenerationStrategy<PropertyModel>
{
    private readonly ILogger<IdPropertySyntaxGenerationStrategy> _logger;
    private readonly ISyntaxService _syntaxService;

    public IdPropertySyntaxGenerationStrategy(
        IServiceProvider serviceProvider,
        ISyntaxService syntaxService,
        ILogger<IdPropertySyntaxGenerationStrategy> logger)

    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _syntaxService = syntaxService ?? throw new ArgumentNullException(nameof(syntaxService));
    }

    public int GetPriority => 1;

    public bool CanHandle(object model, dynamic context = null)
        => model is PropertyModel propertyModel && propertyModel.Id;

    public override async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, PropertyModel model, dynamic context = null)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();


        return builder.ToString();
    }
}

// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Extensions.Logging;
using System.Text;

namespace Endpoint.Core.Syntax.Classes.Strategies;

public class ResponseSyntaxGenerationStrategy : GenericSyntaxGenerationStrategy<ResponseAttributeModel>
{
    private readonly ILogger<ResponseSyntaxGenerationStrategy> _logger;
    public ResponseSyntaxGenerationStrategy(

        ILogger<ResponseSyntaxGenerationStrategy> logger)

    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }



    public override async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, ResponseAttributeModel model)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();


        return builder.ToString();
    }
}

// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Endpoint.Core.Syntax.Classes.Strategies;

public class RequestSyntaxGenerationStrategy : ISyntaxGenerationStrategy<RequestModel>
{
    private readonly ILogger<RequestSyntaxGenerationStrategy> _logger;
    public RequestSyntaxGenerationStrategy(
        IServiceProvider serviceProvider,
        ILogger<RequestSyntaxGenerationStrategy> logger)

    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public int Priority => 0;

    public async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, RequestModel model, dynamic context = null)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();


        return builder.ToString();
    }
}

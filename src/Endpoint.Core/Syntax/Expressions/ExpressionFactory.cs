// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Endpoint.Core.Syntax.Expressions;

public class ExpressionFactory: IExpressionFactory
{
    private readonly ILogger<ExpressionFactory> _logger;

    public ExpressionFactory(ILogger<ExpressionFactory> logger){
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ExpressionModel> CreateAsync()
    {
        _logger.LogInformation("Create Expression.");

        throw new NotImplementedException();
    }

    public async Task<ExpressionModel> LogInformationAsync(string value)
    {
        _logger.LogInformation("Create Expression.");

        var model = new ExpressionModel($"_logger.LogInformation(\"{value}\");");

        return model;
    }
}


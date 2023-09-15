// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Classes;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Endpoint.Core.Syntax.Expressions;

public class ExpressionFactory : IExpressionFactory
{
    private readonly ILogger<ExpressionFactory> _logger;
    private readonly INamingConventionConverter _namingConventionConverter;

    public ExpressionFactory(ILogger<ExpressionFactory> logger, INamingConventionConverter namingConventionConverter)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter));
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

    public async Task<ExpressionModel> ToDtoCreateAsync(ClassModel aggregate)
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.AppendLine($"return new {aggregate.Name}Dto");

        stringBuilder.AppendLine("{");

        foreach (var prop in aggregate.Properties)
        {
            stringBuilder.AppendLine($"{prop.Name} = {_namingConventionConverter.Convert(NamingConvention.CamelCase, aggregate.Name)}.{prop.Name},".Indent(1));
        }

        stringBuilder.AppendLine("};");


        return new ExpressionModel(stringBuilder.ToString());
    }
}


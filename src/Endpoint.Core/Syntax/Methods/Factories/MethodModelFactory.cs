// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Syntax.Attributes;
using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax.Params;
using Endpoint.Core.Syntax.Types;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Endpoint.Core.Syntax.Expressions;
using System.Text;

namespace Endpoint.Core.Syntax.Methods.Factories;

public class MethodFactory : IMethodFactory
{
    private readonly ILogger<MethodFactory> _logger;
    private readonly INamingConventionConverter _namingConventionConverter;
    private readonly IExpressionFactory _expressionFactory;

    public MethodFactory(
        ILogger<MethodFactory> logger,
        INamingConventionConverter namingConventionConverter,
        IExpressionFactory expressionFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter));
        _expressionFactory = expressionFactory ?? throw new ArgumentNullException(nameof(expressionFactory));
    }



    public MethodModel CreateControllerMethod(string name, string controller, RouteType routeType, string directory)
    {
        return routeType switch
        {
            _ => CreateDefaultControllerMethod(name, controller, routeType, directory)
        };
    }

    public MethodModel CreateDefaultControllerMethod(string name, string controller, RouteType routeType, string directory, string swaggerSummery = "", string swaggerDescription = "", string template = null, Dictionary<string, string> properties = null)
    {
        var nameTitleCase = _namingConventionConverter.Convert(NamingConvention.TitleCase, name);

        swaggerSummery = string.IsNullOrEmpty(swaggerSummery) ? nameTitleCase : swaggerSummery;

        swaggerDescription = string.IsNullOrEmpty(swaggerDescription) ? nameTitleCase : swaggerDescription;

        ClassModel controllerClassmodel = new ClassModel(controller);

        MethodModel methodModel = new MethodModel
        {
            ParentType = controllerClassmodel,
            Async = true,
            AccessModifier = AccessModifier.Public,
            Name = name

        };

        methodModel.ReturnType = TypeModel.CreateTaskOfActionResultOf($"{name}Response");

        methodModel.Params = new List<ParamModel>
        {
            new ParamModel()
            {
                Type = new TypeModel($"{_namingConventionConverter.Convert(NamingConvention.PascalCase,name)}Request"),
                Name = "request",
                Attribute = new AttributeModel() { Name = "FromBody" }
            },
            ParamModel.CancellationToken
        };

        methodModel.Attributes.Add(new SwaggerOperationAttributeModel(swaggerSummery, swaggerDescription));

        methodModel.Attributes.Add(new()
        {
            Name = "HttpGet",
            Template = template,
            Properties = properties ?? new Dictionary<string, string>()
            {
                { "Name", _namingConventionConverter.Convert(NamingConvention.CamelCase,name) }
            }
        });

        methodModel.Attributes.Add(new ProducesResponseTypeAttributeModel("InternalServerError"));

        methodModel.Attributes.Add(new ProducesResponseTypeAttributeModel("BadRequest", "ProblemDetails"));

        methodModel.Attributes.Add(new ProducesResponseTypeAttributeModel("OK", $"{name}Response"));

        methodModel.Body = new Syntax.Expressions.ExpressionModel("return await _mediator.Send(request, cancellationToken);");

        return methodModel;
    }

    public async Task<MethodModel> CreateWorkerExecuteAsync()
    {
        var methodBodyBuilder = new StringBuilder();

        methodBodyBuilder.AppendLine("while (!stoppingToken.IsCancellationRequested)");

        methodBodyBuilder.AppendLine("{");

        methodBodyBuilder.AppendLine("_logger.LogInformation(\"Worker running at: {time}\", DateTimeOffset.Now);".Indent(1));

        methodBodyBuilder.AppendLine("await Task.Delay(1000, stoppingToken);".Indent(1));

        methodBodyBuilder.AppendLine("}");

        return new MethodModel()
        {
            Name = "ExecuteAsync",
            Override = true,
            AccessModifier = AccessModifier.Protected,
            Body = new ExpressionModel(methodBodyBuilder.ToString()),
            Async = true,
            ReturnType = new("Task"),
            Params = new List<ParamModel>
            {
                new ()
                {
                    Name = "stoppingToken",
                    Type = new ("CancellationToken")
                }
            }
        };
    }

    public async Task<MethodModel> ToDtoCreateAsync(ClassModel aggregate)
    {
        var model = new MethodModel()
        {
            Name = "ToDto",
            Static = true,
            ReturnType = new TypeModel($"{aggregate.Name}Dto"),
            Params = new()
            {
                new()
                {
                    ExtensionMethodParam = true,
                    Name = _namingConventionConverter.Convert(NamingConvention.CamelCase, aggregate.Name),
                    Type = new (aggregate.Name)
                }
            },
            Body = await _expressionFactory.ToDtoCreateAsync(aggregate)
        };

        return model;
    }

    public async Task<MethodModel> ToDtosAsyncCreateAsync(ClassModel aggregate)
    {
        var model = new MethodModel()
        {
            Name = "ToDtosAsync",
            Async = true,
            Static = true,
            ReturnType = new("Task")
            {
                GenericTypeParameters = new()
                {
                    new ("List")
                    {
                        GenericTypeParameters = new()
                        {
                            new ($"{aggregate.Name}Dto")
                        }
                    }
                }
            },
            Params = new ()
            {
                new()
                {
                    ExtensionMethodParam = true,
                    Name = _namingConventionConverter.Convert(NamingConvention.CamelCase,aggregate.Name, pluralize: true),
                    Type = new ("IQueryable")
                    {
                        GenericTypeParameters = new ()
                        {
                            new (aggregate.Name)
                        }
                    }
                },
                new()
                {
                    Name = "cancellationToken",
                    Type = new ("CancellationToken"),
                    DefaultValue = "null"
                }
            },
            Body = new ExpressionModel($"return await {_namingConventionConverter.Convert(NamingConvention.CamelCase, aggregate.Name, pluralize: true)}.Select(x => x.ToDto()).ToListAsync(cancellationToken);")
        };

        return model;
    }
}



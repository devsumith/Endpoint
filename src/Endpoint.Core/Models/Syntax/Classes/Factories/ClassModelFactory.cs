// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Enums;
using Endpoint.Core.Models.Syntax.Attributes;
using Endpoint.Core.Models.Syntax.Entities;
using Endpoint.Core.Models.Syntax.Fields;
using Endpoint.Core.Models.Syntax.Methods;
using Endpoint.Core.Models.Syntax.Params;
using Endpoint.Core.Models.Syntax.Properties;
using Endpoint.Core.Models.Syntax.Types;
using Endpoint.Core.Services;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Endpoint.Core.Models.Syntax.Classes.Factories;

public class ClassModelFactory : IClassModelFactory
{
    private readonly INamingConventionConverter _namingConventionConverter;
    private readonly INamespaceProvider _namespaceProvider;
    private readonly IFileProvider _fileProvider;

    public ClassModelFactory(INamingConventionConverter namingConventionConverter, INamespaceProvider namespaceProvider, IFileProvider fileProvider)
    {
        _namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter));
        _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        _namespaceProvider = namespaceProvider ?? throw new ArgumentNullException(nameof(namespaceProvider));
    }

    public ClassModel CreateController(EntityModel model, string directory)
    {
        var csProjPath = _fileProvider.Get("*.csproj", directory);

        var csProjDirectory = Path.GetDirectoryName(csProjPath);

        var rootNamesapce = _namespaceProvider.Get(csProjDirectory).Split('.')[0];

        var classModel = new ClassModel($"{model.Name}Controller");

        classModel.UsingDirectives.Add(new UsingDirectiveModel() { Name = $"{rootNamesapce}.Core.AggregateModel.{model.Name}Aggregate.Commands" });

        classModel.UsingDirectives.Add(new UsingDirectiveModel() { Name = $"{rootNamesapce}.Core.AggregateModel.{model.Name}Aggregate.Queries" });

        classModel.UsingDirectives.Add(new UsingDirectiveModel() { Name = "System.Net" });

        classModel.UsingDirectives.Add(new UsingDirectiveModel() { Name = "System.Threading.Tasks" });

        classModel.UsingDirectives.Add(new UsingDirectiveModel() { Name = "MediatR" });

        classModel.UsingDirectives.Add(new UsingDirectiveModel() { Name = "Microsoft.AspNetCore.Mvc" });

        classModel.UsingDirectives.Add(new UsingDirectiveModel { Name = "System.Net.Mime" });

        classModel.UsingDirectives.Add(new UsingDirectiveModel { Name = "Swashbuckle.AspNetCore.Annotations" });

        classModel.Attributes.Add(new AttributeModel() { Type = AttributeType.ApiController, Name = nameof(AttributeType.ApiController) });

        classModel.Attributes.Add(new AttributeModel() { Type = AttributeType.Route, Name = nameof(AttributeType.Route), Template = "\"api/[controller]\"" });

        classModel.Attributes.Add(new AttributeModel() { Type = AttributeType.Produces, Name = nameof(AttributeType.Produces), Template = "MediaTypeNames.Application.Json" });

        classModel.Attributes.Add(new AttributeModel() { Type = AttributeType.Consumes, Name = nameof(AttributeType.Consumes), Template = "MediaTypeNames.Application.Json" });

        classModel.Fields.Add(new FieldModel()
        {
            Type = new TypeModel("IMediator"),
            Name = "_mediator"

        });

        classModel.Fields.Add(new FieldModel()
        {
            Type = TypeModel.LoggerOf(classModel.Name),
            Name = "_logger"
        });

        classModel.Constructors.Add(new Constructors.ConstructorModel(classModel, classModel.Name)
        {
            Params = new List<ParamModel>()
            {
                new ParamModel()
                {
                    Type = new TypeModel("IMediator"),
                    Name = "mediator"

                },
                new ParamModel()
                {
                    Type = TypeModel.LoggerOf(classModel.Name),
                    Name = "logger"
                }
            }
        });


        foreach (var route in Enum.GetValues<RouteType>())
        {
            if (route == RouteType.Page)
                break;

            classModel.Methods.Add(CreateControllerMethod(classModel, model, route));
        }

        return classModel;
    }

    private MethodModel CreateControllerMethod(ClassModel controller, EntityModel model, RouteType routeType)
    {
        MethodModel methodModel = new MethodModel
        {
            ParentType = controller,
            Async = true,
            AccessModifier = AccessModifier.Public
        };

        var cancellationTokenParam = new ParamModel()
        {
            Type = new TypeModel("CancellationToken"),
            Name = "cancellationToken"
        };

        var entityNameCamelCase = _namingConventionConverter.Convert(NamingConvention.CamelCase, model.Name);

        var entityNamePascalCase = _namingConventionConverter.Convert(NamingConvention.PascalCase, model.Name);

        var entityNamePascalCasePlural = _namingConventionConverter.Convert(NamingConvention.PascalCase, model.Name, pluralize: true);

        var entityIdNameCamelCase = $"{entityNameCamelCase}Id";

        var entityIdNamePascalCase = $"{entityNamePascalCase}Id";

        methodModel.Name = routeType switch
        {
            RouteType.Get => "Get",
            RouteType.GetById => "GetById",
            RouteType.Create => "Create",
            RouteType.Update => "Update",
            RouteType.Delete => "Delete",
            _ => ""
        };

        methodModel.ReturnType = routeType switch
        {
            RouteType.Get => TypeModel.CreateTaskOfActionResultOf($"Get{entityNamePascalCasePlural}Response"),
            RouteType.GetById => TypeModel.CreateTaskOfActionResultOf($"Get{entityNamePascalCase}ByIdResponse"),
            RouteType.Page => TypeModel.CreateTaskOfActionResultOf($"Get{entityNamePascalCasePlural}PageResponse"),
            RouteType.Create => TypeModel.CreateTaskOfActionResultOf($"Create{entityNamePascalCase}Response"),
            RouteType.Update => TypeModel.CreateTaskOfActionResultOf($"Update{entityNamePascalCase}Response"),
            RouteType.Delete => TypeModel.CreateTaskOfActionResultOf($"Delete{entityNamePascalCase}Response"),
            _ => null
        };

        switch (routeType)
        {
            case RouteType.GetById:

                methodModel.Attributes.Add(new SwaggerOperationAttributeModel($"Get {entityIdNamePascalCase}  by id", $"Get {entityIdNamePascalCase} by id"));

                methodModel.Attributes.Add(new AttributeModel()
                {
                    Name = "HttpGet",
                    Template = "\"{toDoId:guid}\"",
                    Properties = new Dictionary<string, string>() {
                    { "Name", $"get{entityIdNamePascalCase}ById" }
                }
                });

                methodModel.Attributes.Add(new ProducesResponseTypeAttributeModel("NotFound", "string"));

                methodModel.Attributes.Add(new ProducesResponseTypeAttributeModel("InternalServerError"));

                methodModel.Attributes.Add(new ProducesResponseTypeAttributeModel("BadRequest", "ProblemDetails"));

                methodModel.Attributes.Add(new ProducesResponseTypeAttributeModel("OK", $"Get{entityNamePascalCase}ByIdResponse"));

                methodModel.Params = new List<ParamModel>
                {
                    new ParamModel { Type = new TypeModel("Guid"), Name = entityIdNameCamelCase, Attribute = new AttributeModel() { Name ="FromRoute" } },
                    cancellationTokenParam
                };

                var methodBodyBuilder = new StringBuilder();

                methodBodyBuilder.AppendLine($"var request = new Get{entityNamePascalCase}ByIdRequest()" + "{" + $"{entityIdNamePascalCase} = {entityIdNameCamelCase}" + "};");

                methodBodyBuilder.AppendLine("");

                methodBodyBuilder.AppendLine("var response = await _mediator.Send(request, cancellationToken);");

                methodBodyBuilder.AppendLine("");

                methodBodyBuilder.AppendLine($"if (response.{entityNamePascalCase} == null)");

                methodBodyBuilder.AppendLine("{");

                methodBodyBuilder.AppendLine($"return new NotFoundObjectResult(request.{entityIdNamePascalCase});".Indent(1));

                methodBodyBuilder.AppendLine("}");

                methodBodyBuilder.AppendLine("");

                methodBodyBuilder.Append("return response;");

                methodModel.Body = methodBodyBuilder.ToString();

                break;

            case RouteType.Get:

                methodModel.Attributes.Add(new SwaggerOperationAttributeModel($"Get {entityNamePascalCasePlural}", $"Get {entityNamePascalCasePlural}"));

                methodModel.Attributes.Add(new AttributeModel()
                {
                    Name = "HttpGet",
                    Properties = new Dictionary<string, string>() {
                    { "Name", $"get{entityNamePascalCasePlural}" }
                }
                });

                methodModel.Attributes.Add(new ProducesResponseTypeAttributeModel("InternalServerError"));

                methodModel.Attributes.Add(new ProducesResponseTypeAttributeModel("BadRequest", "ProblemDetails"));

                methodModel.Attributes.Add(new ProducesResponseTypeAttributeModel("OK", $"Get{entityNamePascalCasePlural}Response"));

                methodModel.Params = new List<ParamModel>
                {
                    cancellationTokenParam
                };

                methodModel.Body = $"return await _mediator.Send(new Get{entityNamePascalCasePlural}Request(), cancellationToken);";

                break;

            case RouteType.Create:

                methodModel.Attributes.Add(new SwaggerOperationAttributeModel($"Create {entityNamePascalCase}", $"Create {entityNamePascalCase}"));

                methodModel.Attributes.Add(new AttributeModel()
                {
                    Name = "HttpPost",
                    Properties = new Dictionary<string, string>() {
                    { "Name", $"create{entityNamePascalCase}" }
                }
                });

                methodModel.Attributes.Add(new ProducesResponseTypeAttributeModel("InternalServerError"));

                methodModel.Attributes.Add(new ProducesResponseTypeAttributeModel("BadRequest", "ProblemDetails"));

                methodModel.Attributes.Add(new ProducesResponseTypeAttributeModel("OK", $"Create{entityNamePascalCase}Response"));

                methodModel.Params = new List<ParamModel>
                {
                    new ParamModel { Type = new TypeModel($"Create{entityNamePascalCase}Request "), Name = "request", Attribute = new AttributeModel() { Name ="FromBody" } },
                    cancellationTokenParam
                };

                methodModel.Body = "return await _mediator.Send(request, cancellationToken);";

                break;

            case RouteType.Update:

                methodModel.Attributes.Add(new SwaggerOperationAttributeModel($"Update {entityIdNamePascalCase}", $"Update {entityIdNamePascalCase}"));

                methodModel.Attributes.Add(new AttributeModel() { Name = "HttpPut", Properties = new Dictionary<string, string>() {
                    { "Name", $"update{entityIdNamePascalCase}" }
                }});

                methodModel.Attributes.Add(new ProducesResponseTypeAttributeModel("InternalServerError"));

                methodModel.Attributes.Add(new ProducesResponseTypeAttributeModel("BadRequest", "ProblemDetails"));

                methodModel.Attributes.Add(new ProducesResponseTypeAttributeModel("OK", $"Update{entityNamePascalCase}Response"));

                methodModel.Params = new List<ParamModel>
                {
                    new ParamModel { Type = new TypeModel($"Update{entityNamePascalCase}Request "), Name = "request", Attribute = new AttributeModel() { Name ="FromBody" } },
                    cancellationTokenParam
                };

                methodModel.Body = "return await _mediator.Send(request, cancellationToken);";
                break;

            case RouteType.Delete:
                methodModel.Attributes.Add(new SwaggerOperationAttributeModel($"Delete {entityNamePascalCase}", $"Delete {entityNamePascalCase}"));

                methodModel.Attributes.Add(new AttributeModel()
                {
                    Name = "HttpDelete",
                    Template = "\"{toDoId:guid}\"",
                    Properties = new Dictionary<string, string>() {
                    { "Name", $"delete{entityNamePascalCase}" }
                }
                });

                methodModel.Attributes.Add(new ProducesResponseTypeAttributeModel("InternalServerError"));

                methodModel.Attributes.Add(new ProducesResponseTypeAttributeModel("BadRequest", "ProblemDetails"));

                methodModel.Attributes.Add(new ProducesResponseTypeAttributeModel("OK", $"Delete{entityNamePascalCase}Response"));

                methodModel.Params = new List<ParamModel>
                {
                    new ParamModel { Type = new TypeModel("Guid"), Name = entityIdNameCamelCase, Attribute = new AttributeModel() { Name ="FromRoute" } },
                    cancellationTokenParam
                };

                methodModel.Body = new StringBuilder().AppendJoin(Environment.NewLine, new string[]
                {
                    $"var request = new Delete{entityNamePascalCase}Request()" + " {" + $"{entityIdNamePascalCase} = {entityIdNameCamelCase}" + " };",
                    "",
                    "return await _mediator.Send(request, cancellationToken);"
                }).ToString();

                break;

        }

        return methodModel;
    }
    public ClassModel CreateEntity(string name, string properties = null)
    {
        var classModel = new ClassModel(name);

        var hasId = false;

        if (!string.IsNullOrEmpty(properties))
        {
            foreach (var property in properties.Split(','))
            {
                var parts = property.Split(':');
                var propName = parts[0];
                var propType = parts[1];

                if (propName == $"{name}Id")
                    hasId = true;

                classModel.Properties.Add(new PropertyModel(classModel, AccessModifier.Public, new TypeModel(propType), propName, PropertyAccessorModel.GetSet));

            }
        }

        if (!hasId)
        {
            classModel.Properties.Add(new PropertyModel(classModel, AccessModifier.Public, new("Guid"), $"{name}Id", PropertyAccessorModel.GetSet));
        }

        return classModel;
    }

    public ClassModel CreateDbContext(string name, List<EntityModel> entities, string serviceName)
    {        
        var dbContext = new DbContextModel(_namingConventionConverter, name, entities, serviceName);

        return dbContext;
    }
}


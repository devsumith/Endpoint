/*// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using DotLiquid;
using Endpoint.Core.Syntax.Constructors;
using Endpoint.Core.Syntax.Methods;
using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Fields;
using Endpoint.Core.Syntax.Params;
using Endpoint.Core.Syntax.Types;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Endpoint.Core.Syntax.Classes;

public class RequestHandlerModel : ClassModel
{
    public RequestHandlerModel(ClassModel entity, RequestModel request, ResponseModel response, RouteType routeType, string microserviceName)
        : base("")
    {
        Implements.Add(new TypeModel("IRequestHandler")
        {
            GenericTypeParameters = new List<TypeModel> { new TypeModel(request.Name), new TypeModel(response.Name) }
        });

        Fields.Add(new FieldModel()
        {
            Type = new TypeModel($"I{microserviceName}DbContext"),
            Name = "_context"
        });

        var methodModel = new MethodModel()
        {
            AccessModifier = AccessModifier.Public,
            ParentType = this,
            Name = "Handle",
            Async = true,
            ReturnType = new TypeModel("Task")
            {
                GenericTypeParameters = new List<TypeModel>()
                {
                    new TypeModel(response.Name)
                }
            },
            Params = new List<ParamModel>()
            {
                new ParamModel() {
                    Type = new TypeModel(request.Name),
                    Name = "request"
                },
                new ParamModel() {
                    Type = new TypeModel("CancellationToken"),
                    Name = "cancellationToken"
                }
            }
        };

        Methods.Add(methodModel);
    }
}

public class DeleteRequestHandlerModel : RequestHandlerModel
{
    public DeleteRequestHandlerModel(ClassModel entity, RequestModel request, ResponseModel response, RouteType routeType, string microserviceName, INamingConventionConverter namingConventionConverter)
        : base(entity, request, response, routeType, microserviceName)
    {
        Name = $"Delete{entity.Name}RequestHandler";

        Fields.Add(FieldModel.LoggerOf(Name));

        var requestHandlerCtor = new ConstructorModel(this, Name);

        requestHandlerCtor.Params.Add(ParamModel.LoggerOf(Name));

        requestHandlerCtor.Params.Add(new ParamModel()
        {
            Type = new TypeModel($"I{microserviceName}DbContext"),
            Name = "context"
        });

        Constructors.Add(requestHandlerCtor);

        var builder = new StringBuilder();

        var entityName = entity.Name;

        var entityNamePascalCasePlural = namingConventionConverter.Convert(NamingConvention.PascalCase, entityName, pluralize: true);

        var entityNameCamelCase = namingConventionConverter.Convert(NamingConvention.CamelCase, entityName); ;

        builder.AppendJoin(Environment.NewLine, new string[]
        {
            $"var {entityNameCamelCase} = await _context.{entityNamePascalCasePlural}.FindAsync(request.{entityName}Id);",
            "",
            $"_context.{entityNamePascalCasePlural}.Remove({entityNameCamelCase});",
            "",
            "await _context.SaveChangesAsync(cancellationToken);",
            "",
            "return new ()",
            "{",
            $"{entityName} = {entityNameCamelCase}.ToDto()".Indent(1),
            "};"

        });

        Methods.Single().Body = new Syntax.Expressions.ExpressionModel(builder.ToString());
    }
}

public class UpdateRequestHandlerModel : RequestHandlerModel
{
    public UpdateRequestHandlerModel(ClassModel entity, RequestModel request, ResponseModel response, RouteType routeType, string microserviceName, INamingConventionConverter namingConventionConverter)
        : base(entity, request, response, routeType, microserviceName)
    {
        Name = $"Update{entity.Name}RequestHandler";

        Fields.Add(FieldModel.LoggerOf(Name));

        var requestHandlerCtor = new ConstructorModel(this, Name);

        requestHandlerCtor.Params.Add(ParamModel.LoggerOf(Name));

        requestHandlerCtor.Params.Add(new ParamModel()
        {
            Type = new TypeModel($"I{microserviceName}DbContext"),
            Name = "context"
        });

        Constructors.Add(requestHandlerCtor);

        var entityName = entity.Name;

        var entityNamePascalCasePlural = namingConventionConverter.Convert(NamingConvention.PascalCase, entityName, pluralize: true);

        var entityNameCamelCase = namingConventionConverter.Convert(NamingConvention.CamelCase, entityName); ;

        var methodBodyBuilder = new StringBuilder();

        methodBodyBuilder.AppendLine($"var {entityNameCamelCase} = await _context.{entityNamePascalCasePlural}.SingleAsync(x => x.{entityName}Id == request.{entityName}Id);");

        methodBodyBuilder.AppendLine("");

        foreach (var property in entity.Properties.Where(x => x.Id == false && x.Name != $"{entityName}Id"))
        {
            methodBodyBuilder.AppendLine($"{entityNameCamelCase}.{property.Name} = request.{property.Name};");
        }

        methodBodyBuilder.AppendLine("");

        methodBodyBuilder.AppendLine("await _context.SaveChangesAsync(cancellationToken);");

        methodBodyBuilder.AppendLine("");

        methodBodyBuilder.AppendLine("return new ()");

        methodBodyBuilder.AppendLine("{");

        methodBodyBuilder.AppendLine($"{entityName} = {entityNameCamelCase}.ToDto()".Indent(1));

        methodBodyBuilder.AppendLine("};");

        Methods.Single().Body = new Syntax.Expressions.ExpressionModel(methodBodyBuilder.ToString());
    }
}

public class CreateRequestHandlerModel : RequestHandlerModel
{
    public CreateRequestHandlerModel(ClassModel entity, RequestModel request, ResponseModel response, RouteType routeType, string microserviceName, INamingConventionConverter namingConventionConverter)
        : base(entity, request, response, routeType, microserviceName)
    {
        Name = $"Create{entity.Name}RequestHandler";

        Fields.Add(FieldModel.LoggerOf(Name));

        var requestHandlerCtor = new ConstructorModel(this, Name);

        requestHandlerCtor.Params.Add(ParamModel.LoggerOf(Name));

        requestHandlerCtor.Params.Add(new ParamModel()
        {
            Type = new TypeModel($"I{microserviceName}DbContext"),
            Name = "context"
        });

        Constructors.Add(requestHandlerCtor);

        var methodBodyBuilder = new StringBuilder();

        var entityNameCamelCase = namingConventionConverter.Convert(NamingConvention.CamelCase, entity.Name);

        methodBodyBuilder.AppendLine($"var {entityNameCamelCase} = new {((SyntaxToken)entity.Name).PascalCase()}();");

        methodBodyBuilder.AppendLine("");

        methodBodyBuilder.AppendLine($"_context.{((SyntaxToken)entity.Name).PascalCasePlural()}.Add({entityNameCamelCase});");

        methodBodyBuilder.AppendLine("");

        foreach (var property in entity.Properties.Where(x => x.Name != $"{entity.Name}Id"))
        {
            methodBodyBuilder.AppendLine($"{entityNameCamelCase}.{property.Name} = request.{property.Name};");
        }

        methodBodyBuilder.AppendLine("");

        methodBodyBuilder.AppendLine("await _context.SaveChangesAsync(cancellationToken);");

        methodBodyBuilder.AppendLine("");

        methodBodyBuilder.AppendLine("return new ()");

        methodBodyBuilder.AppendLine("{");

        methodBodyBuilder.AppendLine($"{((SyntaxToken)entity.Name).PascalCase()} = {entityNameCamelCase}.ToDto()".Indent(1));

        methodBodyBuilder.AppendLine("};");

        Methods.Single().Body = new Syntax.Expressions.ExpressionModel(methodBodyBuilder.ToString());
    }
}


public class GetRequestHandlerModel : RequestHandlerModel
{
    public GetRequestHandlerModel(ClassModel entity, RequestModel request, ResponseModel response, RouteType routeType, string microserviceName, INamingConventionConverter namingConventionConverter)
        : base(entity, request, response, routeType, microserviceName)
    {
        var entityNamePascalCasePlural = namingConventionConverter.Convert(NamingConvention.PascalCase, entity.Name, true);

        Name = $"Get{entityNamePascalCasePlural}RequestHandler";

        Fields.Add(FieldModel.LoggerOf(Name));

        var requestHandlerCtor = new ConstructorModel(this, Name);

        requestHandlerCtor.Params.Add(ParamModel.LoggerOf(Name));

        requestHandlerCtor.Params.Add(new ParamModel()
        {
            Type = new TypeModel($"I{microserviceName}DbContext"),
            Name = "context"
        });

        Constructors.Add(requestHandlerCtor);

        var builder = new StringBuilder();

        builder.AppendLine("return new () {");

        builder.AppendLine($"{entityNamePascalCasePlural} = await _context.{entityNamePascalCasePlural}.AsNoTracking().ToDtosAsync(cancellationToken)".Indent(1));

        builder.AppendLine("};");

        Methods.Single().Body = new Syntax.Expressions.ExpressionModel(builder.ToString());
    }
}

public class GetByIdRequestHandlerModel : RequestHandlerModel
{
    public GetByIdRequestHandlerModel(ClassModel entity, RequestModel request, ResponseModel response, RouteType routeType, string microserviceName, INamingConventionConverter namingConventionConverter)
        : base(entity, request, response, routeType, microserviceName)
    {
        var entityNamePascalCasePlural = namingConventionConverter.Convert(NamingConvention.PascalCase, entity.Name, true);

        Name = $"Get{entity.Name}ByIdRequestHandler";

        Fields.Add(FieldModel.LoggerOf(Name));

        var requestHandlerCtor = new ConstructorModel(this, Name);

        requestHandlerCtor.Params.Add(ParamModel.LoggerOf(Name));

        requestHandlerCtor.Params.Add(new ParamModel()
        {
            Type = new TypeModel($"I{microserviceName}DbContext"),
            Name = "context"
        });

        Constructors.Add(requestHandlerCtor);

        var builder = new StringBuilder();

        builder.AppendLine("return new () {");

        builder.AppendLine($"{entity.Name} = (await _context.{entityNamePascalCasePlural}.AsNoTracking().SingleOrDefaultAsync(x => x.{entity.Name}Id == request.{entity.Name}Id)).ToDto()".Indent(1));

        builder.AppendLine("};");

        Methods.Single().Body = new Syntax.Expressions.ExpressionModel(builder.ToString());
    }
}

public class PageRequestHandlerModel : RequestHandlerModel
{
    public PageRequestHandlerModel(ClassModel entity, RequestModel request, ResponseModel response, RouteType routeType, string microserviceName, INamingConventionConverter namingConventionConverter)
        : base(entity, request, response, routeType, microserviceName)
    {
        var entityNamePascalCasePlural = namingConventionConverter.Convert(NamingConvention.PascalCase, entity.Name, true);

        Name = $"Create{entity.Name}RequestHandler";

        Fields.Add(FieldModel.LoggerOf(Name));

        var requestHandlerCtor = new ConstructorModel(this, Name);

        requestHandlerCtor.Params.Add(ParamModel.LoggerOf(Name));

        requestHandlerCtor.Params.Add(new ParamModel()
        {
            Type = new TypeModel($"I{microserviceName}DbContext"),
            Name = "context"
        });

        Constructors.Add(requestHandlerCtor);

        var builder = new StringBuilder();

        builder.AppendLine($"var query = from {((SyntaxToken)entity.Name).CamelCase()} in _context.{((SyntaxToken)entity.Name).PascalCasePlural()}");

        builder.AppendLine($"select {((SyntaxToken)entity.Name).CamelCase()};".Indent(1));

        builder.AppendLine("");

        builder.AppendLine($"var length = await _context.{((SyntaxToken)entity.Name).PascalCasePlural()}.AsNoTracking().CountAsync();");

        builder.AppendLine("");

        builder.AppendLine($"var {((SyntaxToken)entity.Name).CamelCasePlural()} = await query.Page(request.Index, request.PageSize).AsNoTracking()");

        builder.AppendLine(".Select(x => x.ToDto()).ToListAsync();".Indent(1));

        builder.AppendLine("");

        builder.AppendLine("return new ()");

        builder.AppendLine("{");

        builder.AppendLine("Length = length,".Indent(1));

        builder.AppendLine($"Entities = {((SyntaxToken)entity.Name).CamelCasePlural()}".Indent(1));

        builder.AppendLine("};");

        Methods.Single().Body = new Syntax.Expressions.ExpressionModel(builder.ToString());
    }
}*/
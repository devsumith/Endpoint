using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Syntax.Classes;
using Endpoint.Core.Models.Syntax.Types;
using Endpoint.Core.Services;
using System.Collections.Generic;

namespace Endpoint.Core.Models.Syntax.Entities.Aggregate;

public class AggregateModel
{
    public AggregateModel(INamingConventionConverter namingConventionConverter, string microserviceName, ClassModel aggregate)
    {
        MicroserviceName = microserviceName;
        Aggregate = aggregate;
        AggregateDto = aggregate.CreateDto();
        AggregateExtensions = new DtoExtensionsModel(namingConventionConverter, $"{aggregate.Name}Extensions", aggregate);

        foreach (var routeType in Enum.GetValues<RouteType>()) { 

            switch(routeType)
            {
                case RouteType.Get:
                    Queries.Add(new QueryModel(namingConventionConverter,aggregate,routeType)
                    {

                    });
                    break;

                case RouteType.GetById:
                    Queries.Add(new QueryModel(namingConventionConverter,aggregate,routeType)
                    {

                    });
                    break;

                case RouteType.Page:
                    Queries.Add(new QueryModel(namingConventionConverter,aggregate,routeType)
                    {

                    });
                    break;

                case RouteType.Update:
                    Commands.Add(new CommandModel(namingConventionConverter,aggregate,routeType)
                    {

                    });
                    break;

                case RouteType.Create:
                    Commands.Add(new CommandModel(namingConventionConverter,aggregate,routeType)
                    {

                    });
                    break;

                case RouteType.Delete:
                    Commands.Add(new CommandModel(namingConventionConverter,aggregate,routeType)
                    {

                    });
                    break;

                default:
                    throw new NotSupportedException();
            }
        }
    }

    public string MicroserviceName { get; set; }
    public ClassModel Aggregate { get; set; }
    public ClassModel AggregateDto { get; set; }
    public DtoExtensionsModel AggregateExtensions { get; set; }
    public List<CommandModel> Commands { get; set; }
    public List<QueryModel> Queries { get; set; }

}


public class QueryModel : CqrsBase
{
    public string Name { get; set; }
    public QueryModel(INamingConventionConverter namingConventionConverter, ClassModel entity, RouteType routeType)
    {
        var entityNamePlural = namingConventionConverter.Convert(NamingConvention.PascalCase, entity.Name, pluralize: true);

        Name = routeType switch
        {
            RouteType.Get => $"Get{entityNamePlural}",
            RouteType.GetById => $"Get{entity.Name}ById",
            RouteType.Page => $"Get{entityNamePlural}Page",
            _ => throw new NotSupportedException()
        };

        Request = new ClassModel($"{Name}Request");

        Response = new ClassModel($"{Name}Response");

        Request.Implements.Add(new TypeModel("IRequest")
        {
            GenericTypeParameters = new List<TypeModel> { new TypeModel(Response.Name) }
        });

        RequestHandler = new ClassModel($"{Request.Name}Handler");

        RequestHandler.Implements.Add(new TypeModel("IRequestHandler")
        {
            GenericTypeParameters = new List<TypeModel> { new TypeModel(Request.Name), new TypeModel(Response.Name) }
        });
    }

}

public class CommandModel : CqrsBase
{
    public CommandModel(INamingConventionConverter namingConventionConverter, ClassModel entity, RouteType routeType)
    {
        Name = routeType switch
        {
            RouteType.Create => $"Create{entity.Name}",
            RouteType.Update => $"Update{entity.Name}",
            RouteType.Delete => $"Delete{entity.Name}",
            _ => throw new NotSupportedException()
        };

        Request = new ClassModel($"{Name}Request");

        RequestValidator = new ClassModel($"{Name}RequestValidator");

        RequestValidator.Implements.Add(new TypeModel("AbstractValidator")
        {
            GenericTypeParameters = new List<TypeModel>()
            {
                new TypeModel(Request.Name) {}
            }
        });

        Response = new ClassModel($"{Name}Response");

        Request.Implements.Add(new TypeModel("IRequest")
        {
            GenericTypeParameters = new List<TypeModel> { new TypeModel(Response.Name) }
        });

        RequestHandler = new ClassModel($"{Request.Name}Handler");

        RequestHandler.Implements.Add(new TypeModel("IRequestHandler")
        {
            GenericTypeParameters = new List<TypeModel> { new TypeModel(Request.Name), new TypeModel(Response.Name) }
        });

    }
    public ClassModel RequestValidator { get; set; }
}

public class CqrsBase
{
    public string Name { get; set; }
    public ClassModel Request { get; set; }
    public ClassModel Response { get; set; }
    public ClassModel RequestHandler { get; set; }
}


public class QueryModelSyntaxGenerationStrategy : SyntaxGenerationStrategyBase<QueryModel>
{
    public QueryModelSyntaxGenerationStrategy(IServiceProvider serviceProvider) 
        :base(serviceProvider) { }

    public override string Create(ISyntaxGenerationStrategyFactory syntaxGenerationStrategyFactory, QueryModel model, dynamic configuration = null)
    {
        return "";
    }
}

public class CommandModelSyntaxGenerationStrategy : SyntaxGenerationStrategyBase<CommandModel>
{
    public CommandModelSyntaxGenerationStrategy(IServiceProvider serviceProvider) 
        :base(serviceProvider) { }

    public override string Create(ISyntaxGenerationStrategyFactory syntaxGenerationStrategyFactory, CommandModel model, dynamic configuration = null)
    {
        return "";
    }
}
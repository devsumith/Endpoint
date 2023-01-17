using CSharpFunctionalExtensions;
using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Syntax.Classes;
using Endpoint.Core.Services;
using Endpoint.Core.ValueObjects;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text;

namespace Endpoint.Core.Models.Syntax.Methods.RequestHandlerMethodBodies;

public class PageQueryHandlerMethodGenerationStrategy : MethodSyntaxGenerationStrategy
{
    private readonly INamingConventionConverter _namingConventionConverter;
    public PageQueryHandlerMethodGenerationStrategy(
        IServiceProvider serviceProvider,
        INamingConventionConverter namingConventionConverter,
        ILogger<MethodSyntaxGenerationStrategy> logger)
        : base(serviceProvider, logger)
    {
        _namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter));
    }

    public override bool CanHandle(object model, dynamic context = null)
    {
        if (model is MethodModel methodModel && context?.Entity is ClassModel entity)
        {
            return methodModel.Name == "Handle" && methodModel.Params.FirstOrDefault().Type.Name.EndsWith($"PageRequest");
        }

        return false;
    }

    public override int Priority => int.MaxValue;

    public override string Create(ISyntaxGenerationStrategyFactory syntaxGenerationStrategyFactory, MethodModel model, dynamic context = null)
    {
        var builder = new StringBuilder();

        var entityName = context.Entity.Name;

        builder.AppendLine($"var query = from {((Token)entityName).CamelCase} in _context.{((Token)entityName).PascalCasePlural}");
        
        builder.AppendLine($"select {((Token)entityName).CamelCase};".Indent(1));
        
        builder.AppendLine("");
        
        builder.AppendLine($"var length = await _context.{((Token)entityName).PascalCasePlural}.AsNoTracking().CountAsync();");
        
        builder.AppendLine("");
        
        builder.AppendLine($"var {((Token)entityName).CamelCasePlural} = await query.Page(request.Index, request.PageSize).AsNoTracking()");
        
        builder.AppendLine(".Select(x => x.ToDto()).ToListAsync();".Indent(1));
        
        builder.AppendLine("");
        
        builder.AppendLine("return new ()");
        
        builder.AppendLine("{");
        
        builder.AppendLine("Length = length,".Indent(1));
        
        builder.AppendLine($"Entities = {((Token)entityName).CamelCasePlural}".Indent(1));
        
        builder.AppendLine("};");

        model.Body = builder.ToString();

        return base.Create(syntaxGenerationStrategyFactory, model);
    }
}
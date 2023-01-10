using Endpoint.Core.Abstractions;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text;

namespace Endpoint.Core.Models.Syntax.Constructors;

public class ConstructorSyntaxGenerationStrategy : SyntaxGenerationStrategyBase<ConstructorModel>
{
    private readonly ILogger<ConstructorSyntaxGenerationStrategy> _logger;
    private readonly INamingConventionConverter _namingConventionConverter;

    public ConstructorSyntaxGenerationStrategy(
        IServiceProvider serviceProvider,
        INamingConventionConverter namingConventionConverter,
        ILogger<ConstructorSyntaxGenerationStrategy> logger) 
        : base(serviceProvider)
    {
        _namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override string Create(ISyntaxGenerationStrategyFactory syntaxGenerationStrategyFactory, ConstructorModel model, dynamic configuration = null)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        builder.Append(syntaxGenerationStrategyFactory.CreateFor(model.AccessModifier));

        builder.Append($" {model.Name}");

        builder.Append('(');

        builder.Append(string.Join(',', model.Params.Select(x => syntaxGenerationStrategyFactory.CreateFor(x))));

        builder.Append(')');

        if(model.BaseParams.Count > 0)
        {
            builder.AppendLine($": base({ string.Join(',', model.BaseParams)})".Indent(1));
        }

        builder.AppendLine("{");


        foreach(var param in model.Params) {

            var field = model.Class.Fields.SingleOrDefault(x => x.Type.Name == param.Type.Name);

            if(field != null)
            {
                var fieldName = _namingConventionConverter.Convert(NamingConvention.CamelCase, field.Name);

                var paramName = _namingConventionConverter.Convert(NamingConvention.CamelCase, param.Name);

                builder.AppendLine($"{fieldName} = {paramName} ?? throw new ArgumentNullException(nameof({paramName}));".Indent(1));
            }       
        }

        if(!string.IsNullOrEmpty(model.Body))
        {
            builder.AppendLine(model.Body.Indent(1));
        }

        builder.Append("}");

        return builder.ToString();
    }
}
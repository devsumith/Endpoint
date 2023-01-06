using Endpoint.Core.Abstractions;
using Endpoint.Core.Enums;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text;

namespace Endpoint.Core.Models.Syntax.Classes;

public class ClassSyntaxGenerationStrategy : SyntaxGenerationStrategyBase<ClassModel>
{
    private readonly ILogger<ClassSyntaxGenerationStrategy> _logger;
    public ClassSyntaxGenerationStrategy(
        IServiceProvider serviceProvider,
        ILogger<ClassSyntaxGenerationStrategy> logger) 
        : base(serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override string Create(ISyntaxGenerationStrategyFactory syntaxGenerationStrategyFactory, ClassModel model, dynamic configuration = null)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        var accessModifer = model.AccessModifier switch
        {
            AccessModifier.Public => "public",
            AccessModifier.Protected => "protected",
            _ => "public"
        };

        builder.Append(accessModifer);

        if (model.Static)
            builder.Append(" static");

        builder.Append($" {model.Name}");

        if (model.Implements.Count > 0)
        {
            builder.Append(": ");

            builder.Append(string.Join(',', model.Implements.Select(x => syntaxGenerationStrategyFactory.CreateFor(x, configuration))));
        }

        if(model.Properties.Count + model.Methods.Count + model.Constructors.Count + model.Fields.Count == 0)
        {
            builder.Append(" { }");

            return builder.ToString();
        }

        builder.AppendLine($"");

        builder.AppendLine("{");
        
        foreach(var field in model.Fields)
        {
            builder.AppendLine(syntaxGenerationStrategyFactory.CreateFor(field, configuration).Indent(1));
        }

        foreach(var ctor in model.Constructors)
        {
            builder.AppendLine(syntaxGenerationStrategyFactory.CreateFor(ctor, configuration).Indent(1));
        }

        foreach(var property in model.Properties)
        {
            builder.AppendLine(syntaxGenerationStrategyFactory.CreateFor(property, configuration).Indent(1));
        }

        foreach(var method in model.Methods)
        {
            builder.AppendLine(syntaxGenerationStrategyFactory.CreateFor(method, configuration).Indent(1));
        }

        builder.AppendLine("}");
        
        return builder.ToString();
    }
}


using Endpoint.Core.Syntax.Attributes;
using System.Text;


namespace Endpoint.Core.Strategies.CSharp.Attributes;

public class HttpAttributeGenerationStrategy : IAttributeGenerationStrategy
{
    public bool CanHandle(AttributeModel model) => model.Type == AttributeType.Http;

    public string Create(AttributeModel model)
    {
        var properties = new StringBuilder();

        foreach (var property in model.Properties)
        {
            properties.Append($"{property.Key} = \"{property.Value}\"");
        }

        if (!string.IsNullOrEmpty(model.Template))
        {
            return string.Join(Environment.NewLine, new string[1]
            {
                $"[\"{model.Template}\", {model.Name}({properties})]"
            });
        }

        return string.Join(Environment.NewLine, new string[1]
        {
            $"[{model.Name}({properties})]"
        });
    }
}


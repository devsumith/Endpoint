using Endpoint.Core.Models.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core.Strategies.CSharp.Attributes
{
    public class SwaggerOperationAttributeGenerationStrategy : IAttributeGenerationStrategy
    {
        public bool CanHandle(AttributeModel model) => model.Type == AttributeType.SwaggerOperation;

        public string Create(AttributeModel model)
        {
            var content = new List<string>()
            {
                "[SwaggerOperation("
            };

            for(var i = 0; i < model.Properties.Count; i++)
            {
                var property = model.Properties.ElementAt(i);

                if (i + 1 == model.Properties.Count)
                {
                    content.Add($"{property.Key} = \"{property.Value}\"".Indent(1));

                    content.Add(")]");
                }
                else
                {
                    content.Add($"{property.Key} = \"{property.Value}\",".Indent(1));
                }
            }

            return string.Join(Environment.NewLine, content);
        }
    }
}

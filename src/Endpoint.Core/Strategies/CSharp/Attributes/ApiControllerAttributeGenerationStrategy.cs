using Endpoint.Core.Models.Syntax;
using System;

namespace Endpoint.Core.Strategies.CSharp.Attributes
{
    public class ApiControllerAttributeGenerationStrategy : IAttributeGenerationStrategy
    {
        public bool CanHandle(AttributeModel model) => model.Type == AttributeType.ApiController;

        public string[] Create(AttributeModel model) => new string[1] { "[ApiController]" };
    }
}

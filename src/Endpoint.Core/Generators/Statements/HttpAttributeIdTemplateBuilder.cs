using Endpoint.Core.Models;
using Endpoint.Core.ValueObjects;

namespace Endpoint.Core.Builders.Statements
{
    public static class HttpAttributeIdTemplateBuilder
    {
        public static string Build(Settings settings, string resourceName)
        {
            var idPropertyName = settings.IdFormat == IdFormat.Long ? $"{((Token)resourceName).CamelCase}Id": "id";
            var idDotNetType = settings.IdDotNetType == IdDotNetType.Guid ? "guid" : "int";
            return "{" + idPropertyName + ":" + idDotNetType + "}";
        }
    }
}

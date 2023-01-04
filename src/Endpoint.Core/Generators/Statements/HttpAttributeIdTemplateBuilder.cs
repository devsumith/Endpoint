using Endpoint.Core.Models.Options;
using Endpoint.Core.Models.Syntax;
using Endpoint.Core.ValueObjects;

namespace Endpoint.Core.Builders.Statements
{
    public static class HttpAttributeIdTemplateBuilder
    {
        public static string Build(SettingsModel settings, string resourceName)
        {
            var idPropertyName = settings.IdFormat == IdFormat.Long ? $"{((Token)resourceName).CamelCase}Id": "id";
            var idDotNetType = settings.IdDotNetType == IdDotNetType.Guid ? "guid" : "int";
            return "{" + idPropertyName + ":" + idDotNetType + "}";
        }
    }
}

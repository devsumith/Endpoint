using Endpoint.Core.Models.Options;
using Endpoint.Core.Models.Syntax;

namespace Endpoint.Core.Builders.Statements
{
    public static class IdDotNetTypeBuilder
    {
        public static string Build(SettingsModel settings, string resourceName, bool forModel = false)
        {
            var type = settings.IdDotNetType == IdPropertyType.Guid ? $"Guid" : "int";

            if(forModel && type == "Guid")
            {
                return $"{resourceName}Id";
            }

            return type;
        }
    }
}

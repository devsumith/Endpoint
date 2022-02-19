using Endpoint.Core.Models;

namespace Endpoint.Core.Builders.Statements
{
    public static class IdDotNetTypeBuilder
    {
        public static string Build(Settings settings, string resourceName, bool forModel = false)
        {
            var type = settings.IdDotNetType == IdDotNetType.Guid ? $"Guid" : "int";

            if(forModel && type == "Guid")
            {
                return $"{resourceName}Id";
            }

            return type;
        }
    }
}

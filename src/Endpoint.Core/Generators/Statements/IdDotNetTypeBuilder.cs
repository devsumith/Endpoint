
using Endpoint.Core.Options;
using Endpoint.Core.Syntax;


namespace Endpoint.Core.Builders.Statements;

public static class IdDotNetTypeBuilder
{
    public static string Build(SettingsModel settings, string resourceName, bool forModel = false)
    {
        var type = settings.IdDotNetType == IdPropertyType.Guid ? $"Guid" : "int";

        if (forModel && type == "Guid")
        {
            return $"{resourceName}Id";
        }

        return type;
    }
}


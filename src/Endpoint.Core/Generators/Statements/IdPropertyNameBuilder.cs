
using Endpoint.Core.Options;
using Endpoint.Core.Syntax;


namespace Endpoint.Core.Builders.Common;

public class IdPropertyNameBuilder
{
    public static string Build(SettingsModel settings, string objectName)
        => settings.IdFormat == IdPropertyFormat.Long ? $"{objectName}Id" : "Id";
}


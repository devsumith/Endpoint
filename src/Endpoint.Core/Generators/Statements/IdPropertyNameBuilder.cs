using Endpoint.Core.Models.Options;
using Endpoint.Core.Models.Syntax;

namespace Endpoint.Core.Builders.Common
{
    public class IdPropertyNameBuilder
    {
        public static string Build(SettingsModel settings, string objectName)
            => settings.IdFormat == IdFormat.Long ? $"{objectName}Id" : "Id";
    }
}

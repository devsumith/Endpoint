using Endpoint.Core.Models;

namespace Endpoint.Core.Builders.Common
{
    public class IdPropertyNameBuilder
    {
        public static string Build(Settings settings, string objectName)
            => settings.IdFormat == IdFormat.Long ? $"{objectName}Id" : "Id";
    }
}

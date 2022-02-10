using Endpoint.Core.Models;

namespace Endpoint.Core.Builders.Statements
{
    public static class IdDotNetTypeBuilder
    {
        public static string Build(Settings settings)
            => settings.IdDotNetType == IdDotNetType.Guid? $"Guid" : "int";
    }
}

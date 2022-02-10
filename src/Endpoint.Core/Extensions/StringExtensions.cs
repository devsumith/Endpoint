using static System.Linq.Enumerable;

namespace Endpoint.Core
{
    public static class StringExtensions
    {
        public static string Indent(this string value, int indent)
            => $"{string.Join("", Range(1, 4 * indent).Select(i => ' '))}{value}";

    }
}

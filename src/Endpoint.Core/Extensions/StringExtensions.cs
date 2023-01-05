using static System.Linq.Enumerable;

namespace System;

public static class StringExtensions
{
    public static string Indent(this string value, int indent)
        => $"{string.Join("", Range(1, 4 * indent).Select(i => ' '))}{value}";

    public static string GetResourceName(this string[] collection, string name)
        => collection.SingleOrDefault(x => x.EndsWith(name)) == null ?
            collection.SingleOrDefault(x => x.EndsWith($".{name}.txt"))
            : collection.SingleOrDefault(x => x.EndsWith(name));

}

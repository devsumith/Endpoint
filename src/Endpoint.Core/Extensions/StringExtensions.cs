using static System.Linq.Enumerable;

namespace System;

public static class StringExtensions
{
    public static string Indent(this string value, int indent)
    {
        try
        {
            string[] values = value.Split(Environment.NewLine);

            var result = string.Join(Environment.NewLine, values.Select(v => string.IsNullOrEmpty(v) ? v : $"{string.Join("", Range(1, 4 * indent).Select(i => ' '))}{v}"));

            return result;
        } catch (Exception e){
            throw e;
        }

    }

    public static string Remove(this string value, string item) => value.Replace(item, string.Empty);

    public static string GetResourceName(this string[] collection, string name)
        => collection.SingleOrDefault(x => x.EndsWith(name)) == null ?
            collection.SingleOrDefault(x => x.EndsWith($".{name}.txt"))
            : collection.SingleOrDefault(x => x.EndsWith(name));

}

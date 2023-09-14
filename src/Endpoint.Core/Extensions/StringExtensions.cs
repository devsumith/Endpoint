// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Syntax.Cqrs;
using System.Runtime.CompilerServices;
using static System.Linq.Enumerable;

namespace System;

public static class StringExtensions
{
    public static string Indent(this string value, int indent, int spaces = 4)
    {
        try
        {
            string[] values = value.Split(Environment.NewLine);

            var result = string.Join(Environment.NewLine, values.Select(v => string.IsNullOrEmpty(v) ? v : $"{string.Join("", Range(1, spaces * indent).Select(i => ' '))}{v}"));

            return result;
        }
        catch (Exception e)
        {
            throw;
        }

    }

    public static string Remove(this string value, string item) => value.Replace(item, string.Empty);

    public static string GetResourceName(this string[] collection, string name)
        => collection.SingleOrDefault(x => x.EndsWith(name)) == null ?
            collection.SingleOrDefault(x => x.EndsWith($".{name}.txt"))
            : collection.SingleOrDefault(x => x.EndsWith(name));

    public static Tuple<string, string> GetNameAndType(this string value)
    {
        var parts = value.Split(':');

        if (parts.Length == 1)
            return new Tuple<string, string>(value, string.Empty);

        return new Tuple<string, string>(parts[0], parts[1]);
    }

    public static ResponseType ToResponseType(this string value)
    {
        return value.ToLower() switch
        {
            "get" => ResponseType.Get,
            "getbyId" => ResponseType.GetById,
            "page" => ResponseType.Page,
            "create" => ResponseType.Create,
            "delete" => ResponseType.Delete,
            "update" => ResponseType.Update,
            _ => throw new InvalidOperationException()
        };
    }
}


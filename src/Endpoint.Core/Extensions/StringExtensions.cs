// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Properties;
using Endpoint.Core.Syntax.Types;
using Endpoint.Core.Syntax.Units;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static System.Linq.Enumerable;

namespace System;

public static class StringExtensions
{
    private static INamingConventionConverter converter = new NamingConventionConverter();

    public static string ToPascalCase(this string value)
    {
        return converter.Convert(NamingConvention.PascalCase, value);
    }

    public static string ToCamelCase(this string value)
    {
        return converter.Convert(NamingConvention.CamelCase, value);
    }

    public static string Indent(this string value, int indent, int spaces = 4)
    {
        try
        {
            string[] values = value.Split(Environment.NewLine);

            var result = string.Join(Environment.NewLine, values.Select(v => string.IsNullOrEmpty(v) ? v : $"{string.Join(string.Empty, Range(1, spaces * indent).Select(i => ' '))}{v}"));

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
        {
            return new Tuple<string, string>(value, string.Empty);
        }

        return new Tuple<string, string>(parts[0], parts[1]);
    }

    public static RequestType ToRequestType(this string value)
    {
        return value.ToLower() switch
        {
            "get" => RequestType.Get,
            "getbyId" => RequestType.GetById,
            "page" => RequestType.Page,
            "create" => RequestType.Create,
            "delete" => RequestType.Delete,
            "update" => RequestType.Update,
            _ => throw new InvalidOperationException()
        };
    }

    public static string ToRequestName(this string value, INamingConventionConverter namingConventionConverter)
    {
        return value.ToLower() switch
        {
            "get" => $"Get{namingConventionConverter.Convert(NamingConvention.PascalCase, value, pluralize: true)}",
            _ => throw new InvalidOperationException()
        };
    }

    public static PropertyModel ToString(this string value, TypeDeclarationModel model, string? defaultValue = null)
    {
        return new PropertyModel(
            model,
            AccessModifier.Public,
            new TypeModel("string"),
            value,
            PropertyAccessorModel.GetSet)
        {
            DefaultValue = defaultValue,
        };
    }
}

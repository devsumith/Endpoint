// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;
using System.Collections.Generic;

namespace Endpoint.Core.Syntax;

public static class SyntaxExtensions
{
    private readonly static NamingConventionConverter namingConventionConverter = new NamingConventionConverter();

    public static Dictionary<string, object> ToTokens(this SyntaxToken token, string propertyName)
    {
        propertyName = propertyName.Substring(propertyName.IndexOf('_') + 1);

        var propertyNameCamelCase = ((SyntaxToken)propertyName).CamelCase();

        return new()
        {
            { $"{propertyName}", token.Value },
            { $"{propertyNameCamelCase}PascalCase", token.PascalCase() },
            { $"{propertyNameCamelCase}PascalCasePlural", token.PascalCasePlural() },
            { $"{propertyNameCamelCase}CamelCase", token.CamelCase() },
            { $"{propertyNameCamelCase}CamelCasePlural", token.CamelCasePlural() },
            { $"{propertyNameCamelCase}SnakeCase", token.SnakeCase() },
            { $"{propertyNameCamelCase}SnakeCasePlural", token.SnakeCasePlural() },
            { $"{propertyNameCamelCase}TitleCase", token.TitleCase() }
        };
    }

    public static string PascalCase(this SyntaxToken value) => namingConventionConverter.Convert(NamingConvention.PascalCase, value);
    public static string PascalCasePlural(this SyntaxToken value) => namingConventionConverter.Convert(NamingConvention.PascalCase, value, pluralize: true);
    public static string CamelCase(this SyntaxToken value) => namingConventionConverter.Convert(NamingConvention.CamelCase, value);
    public static string CamelCasePlural(this SyntaxToken value) => namingConventionConverter.Convert(NamingConvention.CamelCase, value, pluralize: true);
    public static string SnakeCase(this SyntaxToken value) => namingConventionConverter.Convert(NamingConvention.SnakeCase, value);
    public static string SnakeCasePlural(this SyntaxToken value) => namingConventionConverter.Convert(NamingConvention.SnakeCase, value, pluralize: true);
    public static string TitleCase(this SyntaxToken value) => namingConventionConverter.Convert(NamingConvention.TitleCase, value);

}


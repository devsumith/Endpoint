// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.


using Endpoint.Core.Options;
using Endpoint.Core.Syntax;


namespace Endpoint.Core.Builders.Statements;

public static class HttpAttributeIdTemplateBuilder
{
    public static string Build(SettingsModel settings, string resourceName)
    {
        var idPropertyName = settings.IdFormat == IdPropertyFormat.Long ? $"{((SyntaxToken)resourceName).CamelCase}Id" : "id";
        var idDotNetType = settings.IdDotNetType == IdPropertyType.Guid ? "guid" : "int";
        return "{" + idPropertyName + ":" + idDotNetType + "}";
    }
}



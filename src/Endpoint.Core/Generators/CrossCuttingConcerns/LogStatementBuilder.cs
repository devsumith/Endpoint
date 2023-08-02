// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Builders.Common;
using Endpoint.Core.Options;
using Endpoint.Core.Syntax;

namespace Endpoint.Core.Builders;

public class LogStatementBuilder
{

    private int _indent;
    private string _resource;
    private RouteType _routeType;
    private SettingsModel _settings;

    public LogStatementBuilder(SettingsModel settings, string resource, RouteType? routeType = RouteType.Create, int indent = 0)
    {
        _indent = indent;
        _resource = resource ?? throw new System.ArgumentNullException(nameof(resource));
        _routeType = routeType ?? throw new System.ArgumentNullException(nameof(routeType));
        _settings = settings ?? throw new System.ArgumentNullException(nameof(settings));
    }

    public string[] BuildForCreateCommand()
        => new string[4]
        {
            "_logger.LogInformation(".Indent(_indent),
            "\"----- Sending command: {CommandName}: ({@Command})\",".Indent(_indent + 1),
            $"nameof(Create{((SyntaxToken)_resource).PascalCase}Request),".Indent(_indent + 1),
            "request);".Indent(_indent + 1)
        };

    public string[] BuildForUpdateCommand()
        => new string[6]
        {
            "_logger.LogInformation(".Indent(_indent),
            "\"----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})\",".Indent(_indent + 1),
            $"nameof(Update{((SyntaxToken)_resource).PascalCase}Request),".Indent(_indent + 1),
            $"nameof(request.{((SyntaxToken)_resource).PascalCase}.{IdPropertyNameBuilder.Build(_settings,_resource)}),".Indent(_indent + 1),
            $"request.{((SyntaxToken)_resource).PascalCase}.{IdPropertyNameBuilder.Build(_settings,_resource)},".Indent(_indent + 1),
            "request);".Indent(_indent + 1)
        };

    public string[] BuildForDeleteCommand()
        => new string[6]
        {
            "_logger.LogInformation(".Indent(_indent),
            "\"----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})\",".Indent(_indent + 1),
            $"nameof(Remove{((SyntaxToken)_resource).PascalCase}Request),".Indent(_indent + 1),
            $"nameof(request.{IdPropertyNameBuilder.Build(_settings,_resource)}),".Indent(_indent + 1),
            $"request.{IdPropertyNameBuilder.Build(_settings,_resource)},".Indent(_indent + 1),
            "request);".Indent(_indent + 1)
        };

    public string[] Build()
    {
        switch (_routeType)
        {
            case RouteType.Create:
                return BuildForCreateCommand();


            case RouteType.Update:
                return BuildForUpdateCommand();


            case RouteType.Delete:
                return BuildForDeleteCommand();
        }

        return System.Array.Empty<string>();
    }
}


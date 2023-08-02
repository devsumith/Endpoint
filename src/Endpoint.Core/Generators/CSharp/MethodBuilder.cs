// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.


using Endpoint.Core;
using System.Collections.Generic;
using System.Linq;
using Endpoint.Core.Builders.Common;
using Endpoint.Core.Builders.Statements;
using Endpoint.Core.Syntax;
using Endpoint.Core.Options;


namespace Endpoint.Core.Builders;

public class MethodBuilder
{
    private List<string> _contents;
    private int _indent;
    private AccessModifier _accessModifier;
    private List<string> _attributes;
    public List<string> _body;
    private bool _authorize;
    private RouteType _routeType;
    private string _resource;
    private bool _static;
    private string _name;
    private string _returnType;
    private List<string> _parameters;
    private bool _async;
    private bool _override;
    private SettingsModel _settings;
    public MethodBuilder()
    {
        _accessModifier = AccessModifier.Public;
        _contents = new();
        _attributes = new();
        _body = new();
        _authorize = false;
        _static = false;
        _name = "";
        _returnType = "";
        _parameters = new();
        _async = false;
        _override = false;
    }

    public MethodBuilder WithSettings(SettingsModel settings)
    {
        _settings = settings;
        return this;
    }

    public MethodBuilder WithOverride(bool @override = true)
    {
        _override = @override;
        return this;
    }

    public MethodBuilder WithAccessModifier(AccessModifier accessModifier)
    {
        _accessModifier = accessModifier;
        return this;
    }

    public MethodBuilder WithBody(List<string> body)
    {
        _body = body;

        return this;
    }

    public MethodBuilder WithAsync(bool @async)
    {
        _async = async;

        return this;
    }

    public MethodBuilder WithParameter(string paremeter)
    {
        _parameters.Add(paremeter);

        return this;
    }
    public MethodBuilder WithReturnType(string returnType)
    {
        _returnType = returnType;
        return this;
    }

    public MethodBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public MethodBuilder WithAuthorize(bool authorize)
    {
        this._authorize = authorize;
        return this;
    }

    public MethodBuilder WithPropertyName(string propertName)
    {
        return this;
    }

    public MethodBuilder IsStatic(bool isStatic = true)
    {
        this._static = isStatic;
        return this;
    }

    public MethodBuilder WithEndpointType(RouteType routeType)
    {
        _routeType = routeType;
        return this;
    }

    public MethodBuilder WithResource(string resource)
    {
        _resource = resource;
        return this;
    }

    public MethodBuilder WithIndent(int indent)
    {
        _indent = indent;
        return this;
    }

    public MethodBuilder WithAttribute(string attribute)
    {
        _attributes.Add(attribute);

        return this;
    }
    public string[] Build()
    {
        if (_routeType != default)
        {
            var requestType = _routeType switch
            {
                RouteType.Create => $"Create{((SyntaxToken)_resource).PascalCase}",
                RouteType.Delete => $"Remove{((SyntaxToken)_resource).PascalCase}",
                RouteType.Get => $"Get{((SyntaxToken)_resource).PascalCasePlural}",
                RouteType.GetById => $"Get{((SyntaxToken)_resource).PascalCase}ById",
                RouteType.Update => $"Update{((SyntaxToken)_resource).PascalCase}",
                RouteType.Page => $"Get{((SyntaxToken)_resource).PascalCasePlural}Page",
                _ => throw new System.NotImplementedException()
            };

            _contents = GenericAttributeGenerationStrategy.EndpointAttributes(_settings, _routeType, _resource, _authorize).ToList();

            var methodBuilder = new MethodSignatureBuilder()
                .WithEndpointType(_routeType)
                .WithAsync(true)
                .WithReturnType(TypeBuilder.WithActionResult($"{requestType}Response"));

            if (_routeType == RouteType.GetById || _routeType == RouteType.Delete)
            {
                methodBuilder.WithParameter(new ParameterBuilder(IdDotNetTypeBuilder.Build(_settings, _resource), ((SyntaxToken)$"{IdPropertyNameBuilder.Build(_settings, _resource)}").CamelCase()).WithFrom(From.Route).Build());
            }

            if (_routeType == RouteType.Page)
            {
                methodBuilder.WithParameter(new ParameterBuilder("int", "pageSize").WithFrom(From.Route).Build());

                methodBuilder.WithParameter(new ParameterBuilder("int", "index").WithFrom(From.Route).Build());
            }

            if (_routeType == RouteType.Update)
            {
                methodBuilder.WithParameter(new ParameterBuilder($"{requestType}Request", "request").WithFrom(From.Body).Build());
            }

            if (_routeType == RouteType.Create)
            {
                methodBuilder.WithParameter(new ParameterBuilder($"{requestType}Request", "request").WithFrom(From.Body).Build());
            }

            methodBuilder.WithParameter(new ParameterBuilder("CancellationToken", "cancellationToken").Build());

            _contents.Add(methodBuilder.Build());

            _contents = _contents.Concat(new MethodBodyBuilder(_settings, _routeType, _indent, _resource).Build()).ToList();

            return _contents.ToArray();
        }

        foreach (var attribute in _attributes)
        {
            _contents.Add(attribute);
        }

        var methodSignatureBuilder = new MethodSignatureBuilder()
            .WithName(_name)
            .WithAccessModifier(_accessModifier)
            .WithOverride(_override)
            .IsStatic(_static)
            .WithAsync(_async);

        foreach (var parameter in _parameters)
        {
            methodSignatureBuilder.WithParameter(parameter);
        }

        methodSignatureBuilder.WithReturnType(_returnType);

        _contents.Add(methodSignatureBuilder.Build());


        if (_body.Count > 0)
        {
            _contents.Add("{");
            foreach (var line in _body)
            {
                _contents.Add(line.Indent(_indent + 1));
            }
            _contents.Add("}");
        }

        return _contents.ToArray();
    }
}



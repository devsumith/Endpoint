// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.



using Endpoint.Core.Services;
using System.Collections.Generic;
using Endpoint.Core;
using Endpoint.Core.Syntax;


namespace Endpoint.Core.Builders;

public class GetPageBuilder
{
    private readonly List<string> _content;
    private readonly IContext _context;
    private readonly IFileSystem _fileSystem;
    private string _entity;
    private string _dbContext;
    private string _directory;
    private string _namespace;
    private string _domainNamespace;
    private string _applicationNamespace;

    public GetPageBuilder(IContext context, IFileSystem fileSystem)
    {
        _content = new();
        _context = context;
        _fileSystem = fileSystem;
    }

    public GetPageBuilder WithDomainNamespace(string domainNamespace)
    {
        _domainNamespace = domainNamespace;
        return this;
    }

    public GetPageBuilder WithApplicationNamespace(string applicationNamespace)
    {
        _applicationNamespace = applicationNamespace;
        return this;
    }

    public GetPageBuilder WithDirectory(string directory)
    {
        _directory = directory;
        return this;
    }

    public GetPageBuilder WithEntity(string entity)
    {
        _entity = entity;
        return this;
    }

    public GetPageBuilder WithDbContext(string dbContext)
    {
        _dbContext = dbContext;
        return this;
    }

    public GetPageBuilder WithNamespace(string @namespace)
    {
        _namespace = @namespace;
        return this;
    }

    public void Build()
    {

        var request = new ClassBuilder($"Get{((SyntaxToken)_entity).PascalCasePlural}PageRequest", _context, _fileSystem)
            .WithInterface(new TypeBuilder().WithGenericType("IRequest", $"Get{((SyntaxToken)_entity).PascalCasePlural}PageResponse").Build())
            .WithProperty(new PropertyBuilder().WithType("int").WithName("PageSize").WithAccessors(new AccessorsBuilder().Build()).Build())
            .WithProperty(new PropertyBuilder().WithType("int").WithName("Index").WithAccessors(new AccessorsBuilder().Build()).Build())
            .Class;

        var response = new ClassBuilder($"Get{((SyntaxToken)_entity).PascalCasePlural}PageResponse", _context, _fileSystem)
            .WithBase("ResponseBase")
            .WithProperty(new PropertyBuilder().WithType("int").WithName("Length").WithAccessors(new AccessorsBuilder().Build()).Build())
            .WithProperty(new PropertyBuilder().WithType(new TypeBuilder().WithGenericType("List", $"{((SyntaxToken)_entity).PascalCase}Dto").Build()).WithName($"Entities").WithAccessors(new AccessorsBuilder().Build()).Build())
            .Class;

        var handler = new ClassBuilder($"Get{((SyntaxToken)_entity).PascalCasePlural}PageHandler", _context, _fileSystem)
            .WithBase(new TypeBuilder().WithGenericType("IRequestHandler", $"Get{((SyntaxToken)_entity).PascalCasePlural}PageRequest", $"Get{((SyntaxToken)_entity).PascalCasePlural}PageResponse").Build())
            .WithDependency($"I{((SyntaxToken)_dbContext).PascalCase}", "context")
            .WithDependency($"ILogger<Get{((SyntaxToken)_entity).PascalCasePlural}PageHandler>", "logger")
            .WithMethod(new MethodBuilder().WithName("Handle").WithAsync(true)
            .WithReturnType(new TypeBuilder().WithGenericType("Task", $"Get{((SyntaxToken)_entity).PascalCasePlural}PageResponse").Build())
            .WithParameter(new ParameterBuilder($"Get{((SyntaxToken)_entity).PascalCasePlural}PageRequest", "request").Build())
            .WithParameter(new ParameterBuilder("CancellationToken", "cancellationToken").Build())
            .WithBody(new List<string>() {
            $"var query = from {((SyntaxToken)_entity).CamelCase} in _context.{((SyntaxToken)_entity).PascalCasePlural}",
            $"select {((SyntaxToken)_entity).CamelCase};".Indent(1),
            "",
            $"var length = await _context.{((SyntaxToken)_entity).PascalCasePlural}.AsNoTracking().CountAsync();",
            "",
            $"var {((SyntaxToken)_entity).CamelCasePlural} = await query.Page(request.Index, request.PageSize).AsNoTracking()",
            ".Select(x => x.ToDto()).ToListAsync();".Indent(1),
            "",
            "return new ()",
            "{",
            "Length = length,".Indent(1),
            $"Entities = {((SyntaxToken)_entity).CamelCasePlural}".Indent(1),
            "};"
            }).Build())
            .Class;

        new NamespaceBuilder($"Get{((SyntaxToken)_entity).PascalCasePlural}Page", _context, _fileSystem)
            .WithDirectory(_directory)
            .WithNamespace(_namespace)
            .WithUsing("MediatR")
            .WithUsing("System")
            .WithUsing("Microsoft.Extensions.Logging")
            .WithUsing("System.Threading")
            .WithUsing("System.Threading.Tasks")
            .WithUsing("System.Collections.Generic")
            .WithUsing("System.Linq")
            .WithUsing(_domainNamespace)
            .WithUsing($"{_applicationNamespace}.Interfaces")
            .WithUsing("Microsoft.EntityFrameworkCore")
            .WithClass(request)
            .WithClass(response)
            .WithClass(handler)
            .Build();
    }
}



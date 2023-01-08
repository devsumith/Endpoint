using Endpoint.Core.Models.Syntax.Entities;
using Endpoint.Core.Models.Syntax.RouteHandlers;
using Endpoint.Core.Services;
using Endpoint.Core.ValueObjects;
using System.Collections.Generic;
using System.IO;

namespace Endpoint.Core.Models.Artifacts.Files;

public class FileModelFactory: IFileModelFactory
{
    private readonly IRouteHandlerModelFactory _routeHandlerModelFactory;

    private readonly IAggregateRootModelFactory _aggregateRootModelFactory;

    public FileModelFactory(IAggregateRootModelFactory aggregateRootModelFactory, IRouteHandlerModelFactory routeHandlerModelFactory)
    {
        _aggregateRootModelFactory = aggregateRootModelFactory;
        _routeHandlerModelFactory = routeHandlerModelFactory;
    }

    public TemplatedFileModel CreateTemplate(string template, string name, string directory, string extension = "cs", string filename = null, Dictionary<string, object> tokens = null)
    {
        return new TemplatedFileModel() { Template = template, Name = name, Directory = directory, Extension = extension, Tokens = tokens };
    }

    public EntityFileModel Create(EntityModel model, string directory)
    {
        return new EntityFileModel
        {
            Entity = model
        };
    }

    public CSharpTemplatedFileModel CreateCSharp(string template, string @namespace, string name, string directory, Dictionary<string, object> tokens = null)
        => new()
        {
            Template = template,
            Namespace = @namespace,
            Name = name,
            Directory = directory,
            Extension = "cs",
            Tokens = tokens ?? new TokensBuilder()
            .With("Name", (Token)name)
            .With("Namespace", (Token)@namespace)
            .Build()
        };
    public TemplatedFileModel LaunchSettingsJson(string projectDirectory, string projectName, int port)
        => new()
        {
            Template = "LaunchSettings",
            Name = "launchSettings",
            Directory = $"{projectDirectory}{Path.DirectorySeparatorChar}Properties",
            Extension = "json",
            Tokens = new TokensBuilder()
            .With(nameof(projectName), (Token)projectName)
            .With(nameof(port), (Token)$"{port}")
            .With("SslPort", (Token)$"{port + 1}")
            .Build()
        };
    public TemplatedFileModel AppSettings(string projectDirectory, string projectName, string dbContextName)
        => new()
        {
            Template = "AppSettings",
            Name = "appSettings",
            Directory = projectDirectory,
            Extension = "json",
            Tokens = new TokensBuilder()
            .With(nameof(dbContextName), (Token)dbContextName)
            .With("Namespace", (Token)projectName)
            .Build()
        };
    public MinimalApiProgramFileModel MinimalApiProgram(string projectDirectory, string resources, string properties,  string dbContextName)
    {
        var model = new MinimalApiProgramFileModel()
        {
            DbContextName = dbContextName,
            Name = "Program",
            Extension = "cs",
            Directory = projectDirectory,
        };

        model.Usings.Add("Microsoft.EntityFrameworkCore");

        model.Usings.Add("Microsoft.OpenApi.Models");

        model.Usings.Add("System.Reflection");

        foreach (var resource in resources.Split(','))
        {
            var aggregate = _aggregateRootModelFactory.Create(resource, properties);

            model.Entities.Add(aggregate);

            foreach (var routeHandler in _routeHandlerModelFactory.Create(dbContextName, aggregate))
            {
                model.RouteHandlers.Add(routeHandler);
            }
        }

        return model;
    }
}

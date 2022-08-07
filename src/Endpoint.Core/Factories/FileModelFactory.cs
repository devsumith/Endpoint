using Endpoint.Core.Models;
using Endpoint.Core.Models.Files;
using Endpoint.Core.Services;
using Endpoint.Core.ValueObjects;
using System.Collections.Generic;
using System.IO;

namespace Endpoint.Core.Factories
{
    public class FileModelFactory
    {
        public static EntityFileModel Create(Entity model, string directory)
        {
            return new EntityFileModel
            {
                Entity = model
            };
        }
        public static CSharpTemplatedFileModel CreateCSharp(string template, string @namespace, string name, string directory, Dictionary<string, object> tokens = null)
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
        public static TemplatedFileModel LaunchSettingsJson(string projectDirectory, string projectName, int port)
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
        public static TemplatedFileModel AppSettings(string projectDirectory, string projectName, string dbContextName)
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
        public static MinimalApiProgramFileModel MinimalApiProgram(string projectDirectory, string resources, string properties, bool useShortIdProperty, bool useIntIdPropertyType, string dbContextName)
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
                var aggregate = AggregateRootModelFactory.Create(resource, properties, useShortIdProperty, useIntIdPropertyType);

                model.Aggregates.Add(aggregate);

                foreach(var routeHandler in RouteHandlerFactory.Create(dbContextName, aggregate))
                {
                    model.RouteHandlers.Add(routeHandler);
                }
            }

            return model;
        }
    }
}

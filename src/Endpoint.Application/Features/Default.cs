using CommandLine;
using Endpoint.Application.Builders;
using Endpoint.Application.Services;
using Endpoint.Application.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using static Endpoint.Application.Builders.BuilderFactory;
using static System.Text.Json.JsonSerializer;

namespace Endpoint.Application.Features
{
    internal class Default
    {
        [Verb("default")]
        internal class Request : IRequest<Unit>
        {
            [Option('p')]
            public int Port { get; set; } = 5000;

            [Option('n')]
            public string Name { get; set; } = $"DefaultEndpoint-{Guid.NewGuid()}";

            [Option('r')]
            public string Resource { get; set; } = "Foo";

            [Option('m')]
            public bool Monolith { get; set; } = false;

            [Option('d')]
            public string Directory { get; set; } = System.Environment.CurrentDirectory;
        }

        internal class Handler : IRequestHandler<Request, Unit>
        {
            private ICommandService _commandService;
            private IFileSystem _fileSystem;
            private ISolutionBuilder _solutionBuilder;

            public Handler(
                ICommandService commandService, 
                IFileSystem fileSystem,
                ISolutionBuilder solutionBuilder)
            {
                _commandService = commandService;
                _fileSystem = fileSystem;
                _solutionBuilder = solutionBuilder;
            }
            public Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                var solutionDirectory = $"{request.Directory}{Path.DirectorySeparatorChar}{((Token)request.Name).PascalCase}";
                
                var rootSettingsFilePath = $"{solutionDirectory}{Path.DirectorySeparatorChar}{Constants.SettingsFileName}";

                var settings = new Models.Settings(request.Name, request.Resource, solutionDirectory, isMicroserviceArchitecture: !request.Monolith);

                var json = Serialize(settings, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                });

                _fileSystem.WriteAllLines(rootSettingsFilePath, new List<string> { json }.ToArray());

                _solutionBuilder.Build(settings);

                _commandService.Start("git init", settings.RootDirectory);

                var projRef = Create<ApiBuilder>((a, b, c, d) => new(a, b, c, d, settings))
                    .Build();

                Create<ResourceBuilder>((a, b, c, d) => new(a, b, c, d, settings))
                    .Build();

                slnRef.Add(projRef.FullPath);

                slnRef.OpenInVisualStudio();

                projRef.Run();

                return Task.FromResult(new Unit());
            }
        }
    }
}

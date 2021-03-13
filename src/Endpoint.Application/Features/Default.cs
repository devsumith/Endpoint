using CommandLine;
using Endpoint.Application.Builders;
using MediatR;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using static Endpoint.Application.Builders.BuilderFactory;

namespace Endpoint.Application.Features
{
    internal class Default
    {
        [Verb("default")]
        internal class Request : IRequest<Unit> {
            [Option('p')]
            public int Port { get; set; } = 5000;

            [Option('n')]
            public string Name { get; set; } = $"DefaultEndpoint-{Guid.NewGuid()}";

            [Option('r')]
            public string Resource { get; set; } = "Foo";

            [Option('m')]
            public string Method { get; set; } = "Get";

            [Option('d')]
            public string Directory { get; set; } = System.Environment.CurrentDirectory;
        }

        internal class Handler : IRequestHandler<Request, Unit>
        {
            private string _apiProjectName;
            private string _apiProjectNamespace;
            private string _rootNamespace;
            private string _modelsNamespace;
            private string _dbContextName;

            private int _port;
            private string _name;
            private string _resource;
            private string _directory;

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                _port = request.Port;
                _name = request.Name;
                _resource = request.Resource;
                _directory = request.Directory;
                _rootNamespace = _name.Replace("-", "_");

                _apiProjectName = $"{_name}.Api";
                _apiProjectNamespace = $"{_rootNamespace}.Api";
                _modelsNamespace = $"{_rootNamespace}.Api.Models";
                _dbContextName = _name.Split('.').Length > 1 ? $"{_name.Split('.')[1]}DbContext" : $"{_name.Split('.')[0]}DbContext";

                
                var slnRef = Create<SolutionBuilder>((a, b, c, d) => new(a, b, c, d))
                    .WithDirectory(_directory)
                    .WithName(_name)
                    .Build();

                var apiDirectory = $"{slnRef.SrcDirectory}{Path.DirectorySeparatorChar}{_apiProjectName}";

                var projRef = Create<ApiBuilder>((a, b, c, d) => new(a, b, c, d))
                    .SetDirectory(slnRef.SrcDirectory)
                    .WithPort(_port)
                    .WithSslPort(_port + 1)
                    .WithDbContext(_dbContextName)
                    .SetRootNamespace(_rootNamespace)
                    .WithResource(_resource)
                    .WithModelsNamespace(_modelsNamespace)
                    .SetDomainNamespace(_apiProjectNamespace)
                    .SetApplicationNamespace(_apiProjectNamespace)
                    .SetInfrastructureNamespace(_apiProjectNamespace)
                    .SetApiNamespace(_apiProjectNamespace)
                    .SetDomainDirectory(apiDirectory)
                    .SetApplicationDirectory(apiDirectory)
                    .SetInfrastructureDirectory(apiDirectory)
                    .SetApiDirectory(apiDirectory)
                    .WithApiProjectNamespace(_apiProjectNamespace)
                    .WithName(_apiProjectName)
                    .WithStore(_dbContextName)
                    .Build();

                Create<ResourceBuilder>((a, b, c, d) => new (a, b, c, d))
                    .SetDomainDirectory(apiDirectory)
                    .SetInfrastructureDirectory(apiDirectory)
                    .SetApplicationDirectory(apiDirectory)
                    .SetApiDirectory(apiDirectory)
                    .SetDomainNamespace(_apiProjectNamespace)
                    .SetInfrastructureNamespace(_apiProjectNamespace)
                    .SetApplicationNamespace(_apiProjectNamespace)
                    .SetApiNamespace(_apiProjectNamespace)
                    .WithResource(_resource)
                    .WithDbContext(_dbContextName)
                    .Build();

                slnRef.Add(projRef.FullPath);

                slnRef.OpenInVisualStudio();

                projRef.Run();
                
                return new();
            }
        }
    }
}

// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Builders;
using Endpoint.Core.Models.Syntax;
using Endpoint.Core.Services;
using MediatR;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Cli.Commands;

public class GetById
{
    [Verb("get-by-id")]
    public class Request : IRequest
    {
        [Value(0)]
        public string Entity { get; set; }

        [Option('d')]
        public string Directory { get; set; } = System.Environment.CurrentDirectory;
    }

    public class Handler : IRequestHandler<Request>
    {
        private readonly ISettingsProvider _settingsProvder;
        private readonly IFileSystem _fileSystem;

        public Handler(ISettingsProvider settingsProvider, IFileSystem fileSystem)
        {
            _settingsProvder = settingsProvider;
            _fileSystem = fileSystem;
        }

        public async Task Handle(Request request, CancellationToken cancellationToken)
        {
            var settings = _settingsProvder.Get(request.Directory);

            new GetByIdBuilder(settings, new Context(), _fileSystem)
                .WithDirectory($"{settings.ApplicationDirectory}{Path.DirectorySeparatorChar}Features{Path.DirectorySeparatorChar}{((SyntaxToken)request.Entity).PascalCasePlural}")
                .WithDbContext(settings.DbContextName)
                .WithNamespace($"{settings.ApplicationNamespace}.Features")
                .WithApplicationNamespace($"{settings.ApplicationNamespace}")
                .WithDomainNamespace($"{settings.DomainNamespace}")
                .WithEntity(request.Entity)
                .Build();

    
        }
    }
}


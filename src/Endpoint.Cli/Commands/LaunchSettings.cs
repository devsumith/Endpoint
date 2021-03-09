using CommandLine;
using MediatR;
using Endpoint.Cli.Services;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace Endpoint.Cli.Commands
{
    internal class LaunchSettings
    {
        [Verb("launch-settings")]
        internal class Request : IRequest<Unit> {

            [Option('d', Required = false)]
            public string Directory { get; set; } = System.Environment.CurrentDirectory;

            [Value(1)]
            public string Name { get; set; }

            [Value(0)]
            public int Port { get; set; } = 5000;

            [Value(0)]
            public int SslPort { get; set; } = 5001;
        }

        internal class Handler : IRequestHandler<Request, Unit>
        {
            private readonly IFileSystem _fileSystem;
            private readonly ITemplateLocator _templateLocator;
            private readonly ITemplateProcessor _templateProcessor;
            private readonly ITokenBuilder _tokenBuilder;
            private readonly IServiceProvider _serviceProvider;

            public Handler(
                IFileSystem fileSystem,
                ITemplateLocator templateLocator,
                ITemplateProcessor templateProcessor,
                ITokenBuilder tokenBuilder
                )
            {
                _fileSystem = fileSystem;
                _tokenBuilder = tokenBuilder;
                _templateProcessor = templateProcessor;
                _templateLocator = templateLocator;                
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                
                var template = _templateLocator.Get("LaunchSettings");

                var tokens = _tokenBuilder.Build(new Dictionary<string, string> {
                    { "Port", $"{request.Port}" },
                    { "SslPort", $"{request.SslPort}" },
                    { "Name", $"{request.Name}" }
                }, request.Directory);

                var result = _templateProcessor.Process(template, tokens);

                _fileSystem.WriteAllLines($@"{request.Directory}\launchSettings.json", result);

                return new();
            }
        }
    }
}

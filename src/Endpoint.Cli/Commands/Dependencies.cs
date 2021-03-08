using CommandLine;
using Endpoint.Cli.Services;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Cli.Commands
{
    internal class Dependencies
    {
        [Verb("dependencies")]
        internal class Request : IRequest<Unit> {
            [Option('d', Required = false)]
            public string Directory { get; set; } = System.Environment.CurrentDirectory;
            public string Name { get; set; }
        }

        internal class Handler : IRequestHandler<Request, Unit>
        {
            private readonly IFileSystem _fileSystem;
            private readonly ITemplateLocator _templateLocator;
            private readonly ITemplateProcessor _templateProcessor;
            private readonly ITokenBuilder _tokenBuilder;
            private readonly IMediator _mediator;
            private readonly ICommandService _commandService;

            public Handler(
                IFileSystem fileSystem,
                ITemplateLocator templateLocator,
                ITemplateProcessor templateProcessor,
                ITokenBuilder tokenBuilder,
                IMediator mediator,
                ICommandService commandService
                )
            {
                _fileSystem = fileSystem;
                _tokenBuilder = tokenBuilder;
                _templateProcessor = templateProcessor;
                _templateLocator = templateLocator;
                _mediator = mediator;
                _commandService = commandService;
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                var template = _templateLocator.Get("Dependencies");

                var tokens = _tokenBuilder.Build(new Dictionary<string, string> {
                    { "Name", request.Name }
                }, request.Directory);

                var result = _templateProcessor.Process(template, tokens);

                _fileSystem.WriteAllLines($@"{request.Directory}\Dependencies.cs", result);

                return new();
            }
        }
    }
}

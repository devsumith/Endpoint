using CommandLine;
using Endpoint.Application.Services;
using Endpoint.Application.ValueObjects;
using MediatR;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Application.Features
{
    internal class Event
    {
        [Verb("event")]
        internal class Request : IRequest<Unit>
        {
            [Value(0)]
            public string Aggregate { get; set; }
            [Value(1)]
            public string Name { get; set; }
            [Option('d', Required = false)]
            public string Directory { get; set; } = System.Environment.CurrentDirectory;
        }

        internal class Handler : IRequestHandler<Request, Unit>
        {
            private readonly IFileSystem _fileSystem;
            private readonly ITemplateLocator _templateLocator;
            private readonly ITemplateProcessor _templateProcessor;
            private readonly ICommandService _commandService;
            private readonly ISettingsProvider _settingsProvider;

            public Handler(
                IFileSystem fileSystem,
                ITemplateLocator templateLocator,
                ITemplateProcessor templateProcessor,
                ICommandService commandService,
                ISettingsProvider settingsProvider
                )
            {
                _fileSystem = fileSystem;
                _templateProcessor = templateProcessor;
                _templateLocator = templateLocator;
                _commandService = commandService;
                _settingsProvider = settingsProvider;
            }

            public string[] GetTemplate(ITemplateProcessor templateProcessor, Dictionary<string, object> tokens, string aggregate, string eventName, bool append = false)
            {
                if (!append)
                {
                    var template = GetTemplate(templateProcessor, tokens, aggregate, eventName, true)[0];

                    return _templateProcessor.Process(new string[7] {
                        "using BuildingBlocks.EventStore;",
                        "using System;",
                        "",
                        "namespace {{ domainNamespace }}.Events", 
                        "{", 
                        $"{template}" 
                        ,"}"
                    }, tokens);
                }

                if (eventName == $"{aggregate}Removed")
                {
                    return _templateProcessor.Process(new string[1] { $"    public record {aggregate}Removed (DateTime Deleted):Event;" }, tokens);
                }

                if (eventName == $"{aggregate}Created")
                {
                    return _templateProcessor.Process(new string[1] { $"    public record {aggregate}Created (Guid {aggregate}Id):Event;" }, tokens);
                }

                return _templateProcessor.Process(new string[1]
                {
                    "    public record {{ namePascalCase }} (string Value):Event;"
                }, tokens);
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                var settings = _settingsProvider.Get(request.Directory);

                var tokens = new TokensBuilder()
                    .With(nameof(request.Name), (Token)request.Name)
                    .With(nameof(settings.DomainNamespace),(Token)settings.DomainNamespace)
                    .Build();

                var path = $@"{settings.DomainDirectory}\Events\{request.Aggregate}.cs";

                if (!_fileSystem.Exists(path))
                {
                    _fileSystem.WriteAllLines(path, GetTemplate(_templateProcessor, tokens, request.Aggregate, request.Name));
                }
                else
                {
                    var lines = new List<string>();

                    foreach (var line in File.ReadLines(path))
                    {
                        if (line.StartsWith("}"))
                        {
                            lines.Add(GetTemplate(_templateProcessor, tokens, request.Aggregate, request.Name, true)[0]);
                        }

                        lines.Add(line);
                    }

                    _fileSystem.WriteAllLines(path, lines.ToArray());
                }

                return new Unit();
            }
        }
    }
}

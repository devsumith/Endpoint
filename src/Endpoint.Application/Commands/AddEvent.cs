using CommandLine;
using Endpoint.Core;
using Endpoint.Core.Services;
using Endpoint.Core.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Application.Commands
{
    public class AddEvent
    {
        [Verb("add-event")]
        public class Request : IRequest<Unit>
        {
            [Value(0)]
            public string Aggregate { get; set; }
            
            [Value(1)]
            public string Verb { get; set; }

            [Option('d', Required = false)]
            public string Directory { get; set; } = Environment.CurrentDirectory;
        }

        public class Handler : IRequestHandler<Request, Unit>
        {

            private readonly IFileSystem _fileSystem;
            private readonly ICommandService _commandService;
            private readonly ITemplateProcessor _templateProcessor;
            private readonly ITenseConverter _tenseConverter;
            private readonly ISettingsProvider _settingsProvider;
            public Handler(ISettingsProvider settingsProvider, IFileSystem fileSystem, ICommandService commandService, ITemplateProcessor templateProcessor, ITenseConverter tenseConverter)
            {
                _fileSystem = fileSystem;
                _commandService = commandService;
                _templateProcessor = templateProcessor;
                _tenseConverter = tenseConverter;
                _settingsProvider = settingsProvider;
            }

            public Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                var settings = _settingsProvider.Get(request.Directory);

                var aggregateEventsDirectory = $@"{settings.DomainDirectory}{Path.DirectorySeparatorChar}AggregatesModel{Path.DirectorySeparatorChar}{request.Aggregate}Aggregate{Path.DirectorySeparatorChar}DomainEvents";


                var eventName = $"{request.Aggregate}{_tenseConverter.Convert(request.Verb)}";

                if (!Directory.Exists(aggregateEventsDirectory))
                {
                    _fileSystem.CreateDirectory($"{request.Directory}{Path.DirectorySeparatorChar}{aggregateEventsDirectory}");
                }

                _commandService.Start($"endpoint event {request.Aggregate} {eventName}", aggregateEventsDirectory);

                var whenTemplate = new string[4] {
                    "public void When({{ namePascalCase }} {{ nameCamelCase }})".Indent(2),
                    "{".Indent(3),
                    "Value = {{ nameCamelCase }}.Value".Indent(4),
                    "}".Indent(3) };

                var methodTemplate = new string[5] {
                "public {{ entityNamePascalCase }} {{ method }}(string value)".Indent(2),
                "{".Indent(2),
                "Apply(new {{ namePascalCase }}(value));".Indent(3),
                "return this;".Indent(3),
                "}".Indent(2) };

                var tokens = new TokensBuilder()
                    .With("name", (Token)eventName)
                    .With("entityName", (Token)request.Aggregate)
                    .With("method", (Token)request.Verb)
                    .Build();

                var whenResult = _templateProcessor.Process(whenTemplate, tokens);

                var methodResult = _templateProcessor.Process(methodTemplate, tokens);

                var newLines = new List<string>();

                foreach (var line in File.ReadAllLines($@"{settings.DomainDirectory}/Models/{request.Aggregate}.cs"))
                {
                    if (line.Contains("EnsureValidState"))
                    {
                        foreach (var newLine in whenResult)
                        {
                            newLines.Add(newLine);
                        }

                        newLines.Add("");

                        newLines.Add(line);
                    }
                    else if (line.Contains($"public Guid {request.Aggregate}Id"))
                    {
                        foreach (var newLine in methodResult)
                        {
                            newLines.Add(newLine);
                        }

                        newLines.Add("");

                        newLines.Add(line);
                    }
                    else
                    {
                        newLines.Add(line);
                    }

                }

                _fileSystem.WriteAllLines($@"{settings.DomainDirectory}{Path.DirectorySeparatorChar}Models{Path.DirectorySeparatorChar}{((Token)request.Aggregate).PascalCase}.cs", newLines.ToArray());

                return Task.FromResult(new Unit());
            }
        }
    }
}

// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Services;
using Endpoint.Core.Syntax;
using Humanizer;
using MediatR;

namespace Endpoint.Cli.Commands;

[Verb("event-add")]
public class EventAddRequest : IRequest
{
    [Value('a')]
    public string Aggregate { get; set; }

    [Value('v')]
    public string Verb { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = Environment.CurrentDirectory;
}

public class EventAddRequestHandler : IRequestHandler<EventAddRequest>
{
    private readonly IFileSystem fileSystem;
    private readonly ICommandService commandService;
    private readonly ITemplateProcessor templateProcessor;
    private readonly ITenseConverter tenseConverter;
    private readonly ISettingsProvider settingsProvider;

    public EventAddRequestHandler(ISettingsProvider settingsProvider, IFileSystem fileSystem, ICommandService commandService, ITemplateProcessor templateProcessor, ITenseConverter tenseConverter)
    {
        this.fileSystem = fileSystem;
        this.commandService = commandService;
        this.templateProcessor = templateProcessor;
        this.tenseConverter = tenseConverter;
        this.settingsProvider = settingsProvider;
    }

    public async Task Handle(EventAddRequest request, CancellationToken cancellationToken)
    {
        var settings = settingsProvider.Get(request.Directory);

        var aggregateEventsDirectory = $@"{settings.DomainDirectory}{Path.DirectorySeparatorChar}AggregatesModel{Path.DirectorySeparatorChar}{request.Aggregate}Aggregate{Path.DirectorySeparatorChar}DomainEvents";

        var eventName = $"{request.Aggregate}{tenseConverter.Convert(request.Verb)}";

        if (!Directory.Exists(aggregateEventsDirectory))
        {
            fileSystem.Directory.CreateDirectory($"{request.Directory}{Path.DirectorySeparatorChar}{aggregateEventsDirectory}");
        }

        commandService.Start($"endpoint event {request.Aggregate} {eventName}", aggregateEventsDirectory);

        var whenTemplate = new StringBuilder().AppendJoin(Environment.NewLine, new string[4]
        {
            "public void When({{ namePascalCase }} {{ nameCamelCase }})".Indent(2),
            "{".Indent(3),
            "Value = {{ nameCamelCase }}.Value".Indent(4),
            "}".Indent(3),
        }).ToString();

        var methodTemplate = new StringBuilder().AppendJoin(Environment.NewLine, new string[5]
        {
        "public {{ entityNamePascalCase }} {{ method }}(string value)".Indent(2),
        "{".Indent(2),
        "Apply(new {{ namePascalCase }}(value));".Indent(3),
        "return this;".Indent(3),
        "}".Indent(2),
        }).ToString();

        var tokens = new TokensBuilder()
            .With("name", (SyntaxToken)eventName)
            .With("entityName", (SyntaxToken)request.Aggregate)
            .With("method", (SyntaxToken)request.Verb)
            .Build();

        var whenResult = templateProcessor.Process(whenTemplate, tokens);

        var methodResult = templateProcessor.Process(methodTemplate, tokens);

        var newLines = new List<string>();

        foreach (var line in File.ReadAllLines($@"{settings.DomainDirectory}/Models/{request.Aggregate}.cs"))
        {
            if (line.Contains("EnsureValidState"))
            {
                newLines.Add(whenResult);

                newLines.Add(string.Empty);

                newLines.Add(line);
            }
            else if (line.Contains($"public Guid {request.Aggregate}Id"))
            {
                newLines.Add(methodResult);

                newLines.Add(string.Empty);

                newLines.Add(line);
            }
            else
            {
                newLines.Add(line);
            }
        }

        fileSystem.File.WriteAllText(Path.Combine(settings.DomainDirectory, "Models", $"{request.Aggregate.Pascalize()}.cs"), string.Join(Environment.NewLine, newLines));
    }
}

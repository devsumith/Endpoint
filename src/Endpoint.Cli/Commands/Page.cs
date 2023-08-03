/*// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Services;
using Endpoint.Core.Syntax;
using MediatR;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Cli.Commands;

[Verb("page")]
public class PageRequest : IRequest
{
    [Value(0)]
    public string Entity { get; set; }

    [Option('d')]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class PageRequestHandler : IRequestHandler<PageRequest>
{
    private readonly ISettingsProvider _settingsProvder;
    private readonly IFileSystem _fileSystem;

    public PageRequestHandler(ISettingsProvider settingsProvider, IFileSystem fileSystem)
    {
        _settingsProvder = settingsProvider;
        _fileSystem = fileSystem;
    }

    public async Task Handle(PageRequest request, CancellationToken cancellationToken)
    {
        var settings = _settingsProvder.Get(request.Directory);

        new GetPageBuilder(new Endpoint.Core.Services.Context(), _fileSystem)
            .WithDirectory($"{settings.ApplicationDirectory}{Path.DirectorySeparatorChar}Features{Path.DirectorySeparatorChar}{((SyntaxToken)request.Entity).PascalCasePlural}")
            .WithDbContext(settings.DbContextName)
            .WithNamespace($"{settings.ApplicationNamespace}.Features")
            .WithApplicationNamespace($"{settings.ApplicationNamespace}")
            .WithDomainNamespace($"{settings.DomainNamespace}")
            .WithEntity(request.Entity)
            .Build();


    }
}
*/
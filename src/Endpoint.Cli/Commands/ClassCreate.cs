// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts.Files.Services;
using MediatR;

namespace Endpoint.Cli.Commands;

[Verb("class-create")]
public class ClassCreateRequest : IRequest
{
    [Option('n')]
    public string Name { get; set; }

    [Option('p')]
    public string Properties { get; set; }

    [Option('d')]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class ClassCreateRequestHandler : IRequestHandler<ClassCreateRequest>
{
    private readonly IClassService classService;

    public ClassCreateRequestHandler(IClassService classService)
    {
        this.classService = classService;
    }

    public async Task Handle(ClassCreateRequest request, CancellationToken cancellationToken)
    {
        await classService.CreateAsync(request.Name, request.Properties, request.Directory);
    }
}

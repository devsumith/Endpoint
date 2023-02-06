// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Models.Artifacts.Files.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;


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
    private readonly IClassService _classService;

    public ClassCreateRequestHandler(IClassService classService)
    {
        _classService = classService;
    }

    public Task<Unit> Handle(ClassCreateRequest request, CancellationToken cancellationToken)
    {
        _classService.Create(request.Name, request.Properties, request.Directory);

        return Task.FromResult(new Unit());
    }
}


// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using MediatR;

namespace Endpoint.Cli.Commands;

[Verb("add-resource")]
public class AddResourceRequest : IRequest
{
    [Option('r', "resource")]
    public string Resource { get; set; }

    [Option("properties")]
    public string Properties { get; set; }

    [Option('d', "directory")]
    public string Directory { get; set; } = Environment.CurrentDirectory;
}

public class AddResourceHandler : IRequestHandler<AddResourceRequest>
{
    public async Task Handle(AddResourceRequest request, CancellationToken cancellationToken)
    {
        // var options = TinyMapper.Map<AddResourceOptions>(request);

        // AdditionalResourceGenerator.Generate(options, _factory);
    }
}
// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Cli.Commands;

public class AddResource
{
    [Verb("add-resource")]
    public class Request : IRequest
    {
        [Option('r', "resource")]
        public string Resource { get; set; }

        [Option("properties")]
        public string Properties { get; set; }

        [Option('d', "directory")]
        public string Directory { get; set; } = Environment.CurrentDirectory;
    }

    public class Handler : IRequestHandler<Request>
    {
        public async Task Handle(Request request, CancellationToken cancellationToken)
        {
            //var options = TinyMapper.Map<AddResourceOptions>(request);

            //AdditionalResourceGenerator.Generate(options, _factory);
        }
    }
}
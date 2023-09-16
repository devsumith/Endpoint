// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Services;
using MediatR;

namespace Endpoint.Cli.Commands;

public class EntityConfiguration
{
    [Verb("entity-config")]
    public class Request : IRequest
    {
        [Value(0)]
        public string Entity { get; set; }

        [Option('d', Required = false)]
        public string Directory { get; set; } = System.Environment.CurrentDirectory;
    }

    public class Handler : IRequestHandler<Request>
    {
        private readonly ISettingsProvider settingsProvider;
        private readonly IFileSystem fileSystem;

        public Handler(ISettingsProvider settingsProvider, IFileSystem fileSystem)
        {
            this.settingsProvider = settingsProvider;
            this.fileSystem = fileSystem;
        }

        public async Task Handle(Request request, CancellationToken cancellationToken)
        {
            // Create Entity Type Converter
            // EF
        }
    }
}

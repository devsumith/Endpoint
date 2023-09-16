// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Services;
using Endpoint.Core.Syntax;
using MediatR;

namespace Endpoint.Cli.Commands;

public class ApiTest
{
    [Verb("api-test")]
    public class Request : IRequest
    {
        [Option('n', "name")]
        public string EntityName { get; set; }

        [Option('d', Required = false)]
        public string Directory { get; private set; } = System.Environment.CurrentDirectory;
    }

    public class Handler : IRequestHandler<Request>
    {
        private readonly ITemplateLocator templateLocator;
        private readonly ITemplateProcessor templateProcessor;
        private readonly IFileSystem fileSystem;

        public Handler(ITemplateLocator templateLocator, ITemplateProcessor templateProcessor, IFileSystem fileSystem)
        {
            this.templateLocator = templateLocator;
            this.templateProcessor = templateProcessor;
            this.fileSystem = fileSystem;
        }

        public async Task Handle(Request request, CancellationToken cancellationToken)
        {
            var template = templateLocator.Get(nameof(ApiTest));

            var tokens = new TokensBuilder()
                .With(nameof(request.EntityName), (SyntaxToken)request.EntityName)
                .Build();

            var contents = string.Join(Environment.NewLine, templateProcessor.Process(template, tokens));

            fileSystem.File.WriteAllText($@"{request.Directory}/{((SyntaxToken)request.EntityName).PascalCase}ControllerTests.cs", contents);
        }
    }
}

// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Services;
using MediatR;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Cli.Commands;

public class PostApiBuild
{
    [Verb("post-api-build")]
    public class Request : IRequest
    {

        [Option('d', Required = false)]
        public string Directory { get; set; } = Environment.CurrentDirectory;
    }

    public class Handler : IRequestHandler<Request>
    {
        private readonly ICommandService _commandService;
        private readonly ISettingsProvider _settingsProvider;

        public Handler(ICommandService commandService, ISettingsProvider settingsProvider)
        {
            _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
            _settingsProvider = settingsProvider ?? throw new ArgumentNullException(nameof(settingsProvider));
        }

        public async Task Handle(Request request, CancellationToken cancellationToken)
        {
            Core.Models.Options.SettingsModel settings = _settingsProvider.Get(request.Directory);

            var solutionName = settings.ApiNamespace.Replace(".Api", "");

            var appDirectory = $"{settings.RootDirectory}{Path.DirectorySeparatorChar}src{Path.DirectorySeparatorChar}{solutionName}.App";

            var apiSwaggerFilePath = $"{settings.ApiDirectory}{Path.DirectorySeparatorChar}swagger.json";

            var appSwaggerFilePath = $"{appDirectory}{Path.DirectorySeparatorChar}swagger.json";

            if (Directory.Exists($"{settings.RootDirectory}{Path.DirectorySeparatorChar}src{Path.DirectorySeparatorChar}{solutionName}.App"))
            {
                if (File.Exists(appSwaggerFilePath))
                {
                    File.Delete(appSwaggerFilePath);
                }

                if (File.Exists(apiSwaggerFilePath))
                {
                    File.Copy(apiSwaggerFilePath, appSwaggerFilePath);

                    _commandService.Start("node node_modules/ng-swagger-gen/ng-swagger-gen --config ng-swagger-gen.json", appDirectory);
                }
            }


        }
    }
}


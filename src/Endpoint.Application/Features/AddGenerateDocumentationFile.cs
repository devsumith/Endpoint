using CommandLine;
using Endpoint.SharedKernal.Services;
using MediatR;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Application.Features
{
    internal class AddGenerateDocumentationFile
    {
        [Verb("add-generate-documentation-file")]
        internal class Request : IRequest<Unit>
        {
            [Option('d', Required = false)]
            public string Directory { get; set; } = Environment.CurrentDirectory;
        }

        internal class Handler : IRequestHandler<Request, Unit>
        {
            private readonly ICsProjService _csProjService;
            private readonly ISettingsProvider _settingsProvider;

            public Handler(ICsProjService csProjService, ISettingsProvider settingsProvider)
            {
                _csProjService = csProjService;
                _settingsProvider = settingsProvider;
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                var settings = _settingsProvider.Get(request.Directory);

                var apiCsProjPath = $"{settings.ApiDirectory}{Path.DirectorySeparatorChar}{settings.ApiNamespace}.csproj";

                Console.WriteLine(apiCsProjPath);

                _csProjService.AddGenerateDocumentationFile(apiCsProjPath);

                return new();
            }
        }
    }
}

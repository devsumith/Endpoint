using CommandLine;
using Endpoint.Core.Services;
using MediatR;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Application.Commands;

public class AddGenerateDocumentationFile
{
    [Verb("add-generate-documentation-file")]
    public class Request : IRequest<Unit>
    {
        [Option('d', Required = false)]
        public string Directory { get; set; } = Environment.CurrentDirectory;
    }

    public class Handler : IRequestHandler<Request, Unit>
    {
        private readonly ISettingsProvider _settingsProvider;
        private readonly IApiProjectFilesGenerationStrategy _apiProjectFilesGenerationStrategy;

        public Handler(ISettingsProvider settingsProvider, IApiProjectFilesGenerationStrategy apiProjectFilesGenerationStrategy)
        {
            _settingsProvider = settingsProvider ?? throw new ArgumentNullException(nameof(settingsProvider));
            _apiProjectFilesGenerationStrategy = apiProjectFilesGenerationStrategy ?? throw new System.ArgumentNullException(nameof(apiProjectFilesGenerationStrategy));    
        }

        public Task<Unit> Handle(Request request, CancellationToken cancellationToken)
        {
            var settings = _settingsProvider.Get(request.Directory);

            var apiCsProjPath = $"{settings.ApiDirectory}{Path.DirectorySeparatorChar}{settings.ApiNamespace}.csproj";

            _apiProjectFilesGenerationStrategy.AddGenerateDocumentationFile(apiCsProjPath);

            return Task.FromResult(new Unit());
        }
    }
}

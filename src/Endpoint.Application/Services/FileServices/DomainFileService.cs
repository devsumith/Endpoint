using Endpoint.Application.Builders;
using Endpoint.Application.Models;
using Endpoint.Application.ValueObjects;
using System.IO;

namespace Endpoint.Application.Services.FileServices
{
    public class DomainFileService : BaseFileService, IDomainFileService
    {
        public DomainFileService(
            ICommandService commandService,
            ITemplateProcessor templateProcessor,
            ITemplateLocator templateLocator,
            IFileSystem fileSystem) : base(commandService, templateProcessor, templateLocator, fileSystem)
        { }

        public void Build(Settings settings)
        {
            _createFolder($@"{settings.DomainDirectory}{Path.DirectorySeparatorChar}Extensions", settings.DomainDirectory);

            _removeDefaultFiles(settings.DomainDirectory);

            _buildQueryableExtensions(settings);

            _buildResponseBase(settings);
        }

        private void _buildResponseBase(Models.Settings settings)
        {
            var template = _templateLocator.Get(nameof(ResponseBaseBuilder));

            var tokens = new TokensBuilder()
                .With("RootNamespace", (Token)settings.RootNamespace)
                .With("Directory", (Token)settings.DomainDirectory)
                .With("Namespace", (Token)settings.DomainNamespace)
                .Build();

            var contents = _templateProcessor.Process(template, tokens);

            _fileSystem.WriteAllLines($@"{settings.DomainDirectory}/ResponseBase.cs", contents);

            _commandService.Start($"dotnet add package Microsoft.EntityFrameworkCore --version 5.0.10", $@"{settings.DomainDirectory}");
        }

        private void _buildQueryableExtensions(Models.Settings settings)
        {
            var template = _templateLocator.Get(nameof(QueryableExtensionsBuilder));

            var tokens = new TokensBuilder()
                .With("RootNamespace", (Token)settings.RootNamespace)
                .With("Directory", (Token)settings.DomainDirectory)
                .With("Namespace", (Token)settings.DomainNamespace)
                .With("ApplicationNamespace", (Token)settings.DomainNamespace)
                .Build();

            var contents = _templateProcessor.Process(template, tokens);

            _fileSystem.WriteAllLines($@"{settings.DomainDirectory}{Path.DirectorySeparatorChar}Extensions{Path.DirectorySeparatorChar}QueryableExtensions.cs", contents);
        }
    }
}

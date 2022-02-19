using Endpoint.Core.Models;
using Endpoint.Core.ValueObjects;
using System.IO;

namespace Endpoint.Core.Services
{
    public class SharedKernelProjectFilesGenerationStrategy : BaseProjectFilesGenerationStrategy, ISharedKernelProjectFilesGenerationStrategy
    {
        public SharedKernelProjectFilesGenerationStrategy(
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

        private void _buildResponseBase(Settings settings)
        {
            var template = _templateLocator.Get("ResponseBase");

            var tokens = new TokensBuilder()
                .With("RootNamespace", (Token)settings.RootNamespace)
                .With("Directory", (Token)settings.DomainDirectory)
                .With("Namespace", (Token)settings.DomainNamespace)
                .Build();

            var contents = _templateProcessor.Process(template, tokens);

            _fileSystem.WriteAllLines($@"{settings.DomainDirectory}/ResponseBase.cs", contents);

            _commandService.Start($"dotnet add package Microsoft.EntityFrameworkCore --version 6.0.2", $@"{settings.DomainDirectory}");
        }

        private void _buildQueryableExtensions(Endpoint.Core.Models.Settings settings)
        {
            var template = _templateLocator.Get("QueryableExtensions");

            var tokens = new TokensBuilder()
                .With("RootNamespace", (Token)settings.RootNamespace)
                .With("Directory", (Token)settings.DomainDirectory)
                .With("Namespace", (Token)settings.DomainNamespace)
                .With("DomainNamespace", (Token)settings.DomainNamespace)
                .Build();

            var contents = _templateProcessor.Process(template, tokens);

            _fileSystem.WriteAllLines($@"{settings.DomainDirectory}{Path.DirectorySeparatorChar}Extensions{Path.DirectorySeparatorChar}QueryableExtensions.cs", contents);
        }
    }
}

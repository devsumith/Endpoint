using Endpoint.Application.Services;
using Endpoint.Application.ValueObjects;

namespace Endpoint.Application.Builders
{
    public class DependenciesBuilder: BuilderBase<DependenciesBuilder>
    {
        public DependenciesBuilder(
            ICommandService commandService,
            ITemplateProcessor templateProcessor,
            ITemplateLocator templateLocator,
            IFileSystem fileSystem):base(commandService, templateProcessor, templateLocator, fileSystem)
        { }

        private Token _dbContext;
        public DependenciesBuilder WithDbContextName(string dbContextName)
        {
            _dbContext = (Token)dbContextName;
            return this;
        }
        public void Build()
        {
            var template = _templateLocator.Get(nameof(DependenciesBuilder));

            var tokens = new TokensBuilder()
                .With(nameof(_rootNamespace), _rootNamespace)
                .With(nameof(_directory), _directory)
                .With(nameof(_namespace), _namespace)
                .With(nameof(_dbContext), _dbContext)
                .With(nameof(_applicationNamespace), _applicationNamespace)
                .With(nameof(_apiNamespace), _apiNamespace)
                .With(nameof(_domainNamespace), _domainDirectory)
                .With(nameof(_infrastructureNamespace), _infrastructureNamespace)
                .Build();

            var contents = _templateProcessor.Process(template, tokens);

            _fileSystem.WriteAllLines($@"{_directory.Value}/Dependencies.cs", contents);
        }
    }
}

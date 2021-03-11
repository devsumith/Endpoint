using Endpoint.Cli.Services;
using Endpoint.Cli.ValueObjects;

namespace Endpoint.Cli.Builders
{
    public class StartupBuilder: BuilderBase<StartupBuilder>
    {
        public StartupBuilder(
            ICommandService commandService,
            ITemplateProcessor templateProcessor,
            ITemplateLocator templateLocator,
            IFileSystem fileSystem):base(commandService, templateProcessor, templateLocator, fileSystem)
        { }

        private Token _dbContext;
        public StartupBuilder WithDbContextName(string dbContextName)
        {
            _dbContext = (Token)dbContextName;

            return this;
        }

        public void Build()
        {
            var template = _templateLocator.Get(nameof(StartupBuilder));

            var tokens = new TokensBuilder()
                .With(nameof(_rootNamespace), _rootNamespace)
                .With(nameof(_directory), _directory)
                .With(nameof(_namespace), _namespace)
                .With(nameof(_dbContext), _dbContext)
                .Build();

            var contents = _templateProcessor.Process(template, tokens);

            _fileSystem.WriteAllLines($@"{_directory.Value}/Startup.cs", contents);
        }
    }
}

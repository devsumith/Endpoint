using Endpoint.Cli.Services;
using Endpoint.Cli.ValueObjects;

namespace Endpoint.Cli.Builders
{
    public class ProgramBuilder: BuilderBase<ProgramBuilder>
    {
        public ProgramBuilder(
            ICommandService commandService,
            ITemplateProcessor templateProcessor,
            ITemplateLocator templateLocator,
            IFileSystem fileSystem):base(commandService, templateProcessor, templateLocator, fileSystem)
        { }

        private Token _dbContext;
        public ProgramBuilder WithDbContextName(string dbContextName)
        {
            _dbContext = (Token)dbContextName;
            return this;
        }
        public void Build()
        {
            var template = _templateLocator.Get(nameof(ProgramBuilder));

            var tokens = new TokensBuilder()
                .With(nameof(_rootNamespace), _rootNamespace)
                .With(nameof(_directory), _directory)
                .With(nameof(_namespace), _namespace)
                .With(nameof(_dbContext), _dbContext)
                .Build();

            var contents = _templateProcessor.Process(template, tokens);

            _fileSystem.WriteAllLines($@"{_directory.Value}/Program.cs", contents);
        }
    }
}

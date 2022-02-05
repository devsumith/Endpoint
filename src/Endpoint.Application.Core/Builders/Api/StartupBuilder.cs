namespace Endpoint.Application.Builders
{
    public class StartupBuilder
    {
        public void Build()
        {
/*            var template = _templateLocator.Get(nameof(StartupBuilder));

            var tokens = new TokensBuilder()
                .With(nameof(_rootNamespace), _rootNamespace)
                .With(nameof(_directory), _directory)
                .With(nameof(_namespace), _namespace)
                .With(nameof(_dbContext), _dbContext)
                .Build();

            var contents = _templateProcessor.Process(template, tokens);

            _fileSystem.WriteAllLines($@"{_directory.Value}/Startup.cs", contents);*/
        }
    }
}

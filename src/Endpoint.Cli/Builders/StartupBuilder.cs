using Endpoint.Cli.Services;
using Endpoint.Cli.ValueObjects;
using System.Collections.Generic;

namespace Endpoint.Cli.Builders
{
    public class StartupBuilder
    {
        private readonly ICommandService _commandService;
        private readonly ITemplateProcessor _templateProcessor;
        private readonly ITemplateLocator _templateLocator;
        private readonly IFileSystem _fileSystem;

        private Token _directory = (Token)System.Environment.CurrentDirectory;
        private Token _rootNamespace;

        public StartupBuilder(
            ICommandService commandService,
            ITemplateProcessor templateProcessor,
            ITemplateLocator templateLocator,
            IFileSystem fileSystem)
        {
            _commandService = commandService;
            _templateProcessor = templateProcessor;
            _templateLocator = templateLocator;
            _fileSystem = fileSystem;
        }

        public StartupBuilder SetDirectory(string directory)
        {
            _directory = (Token)directory;
            return this;
        }

        public StartupBuilder SetRootNamespace(string rootNamespace)
        {
            _rootNamespace = (Token)rootNamespace;
            return this;
        }

        public void Build()
        {
            var template = _templateLocator.Get(nameof(ProgramBuilder));

            var tokens = new SimpleTokensBuilder()
                .WithToken(nameof(_rootNamespace), _rootNamespace)
                .WithToken(nameof(_directory), _directory)
                .WithToken("Namespace", (Token)$"{_rootNamespace.Value}.Api")
                .Build();

            var contents = _templateProcessor.Process(template, tokens);

            _fileSystem.WriteAllLines($@"{_directory.Value}/Program.cs", contents);
        }
    }
}

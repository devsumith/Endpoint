using Endpoint.Cli.Services;
using Endpoint.Cli.ValueObjects;
using System.Collections.Generic;
using System.IO;
using static System.Array;

namespace Endpoint.Cli.Builders
{
    public class DbContextBuilder
    {
        private readonly ICommandService _commandService;
        private readonly ITemplateProcessor _templateProcessor;
        private readonly ITemplateLocator _templateLocator;
        private readonly IFileSystem _fileSystem;

        private Token _directory = (Token)System.Environment.CurrentDirectory;
        private Token _modelsDirectory;
        private Token _rootNamespace;
        private List<Token> _models = new List<Token>();
        
        public DbContextBuilder(
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

        public DbContextBuilder SetDirectory(string directory)
        {
            _directory = (Token)directory;
            return this;
        }

        public DbContextBuilder WithModel(string model)
        {
            _models.Add((Token)model);

            return this;
        }

        public DbContextBuilder SetModelsDirectory(string modelsDirectory)
        {
            _modelsDirectory = (Token)modelsDirectory;
            return this;
        }

        public DbContextBuilder SetRootNamespace(string rootNamespace)
        {
            _rootNamespace = (Token)rootNamespace;
            return this;
        }

        public void Build()
        {
            var template = _templateLocator.Get(nameof(DbContextBuilder));

            var interfaceTemplate = _templateLocator.Get("DbContextInterfaceBuilder");

            var tokens = new SimpleTokensBuilder()
                .WithToken(nameof(_rootNamespace), _rootNamespace)
                .WithToken(nameof(_directory), _directory)
                .WithToken("Namespace", (Token)$"{_rootNamespace.Value}.Api.Data")
                .WithToken("dbContext",(Token)$"{_rootNamespace.Value}DbContext")
                .Build();

            var contents = _templateProcessor.Process(template, tokens);

            var interfaceContents = _templateProcessor.Process(interfaceTemplate, tokens);

            _fileSystem.WriteAllLines($@"{_directory.Value}/{_rootNamespace.PascalCase}DbContext.cs", contents);

            _fileSystem.WriteAllLines($@"{_directory.Value}/I{_rootNamespace.PascalCase}DbContext.cs", interfaceContents);
        }
    }
}

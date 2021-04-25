using Endpoint.Application.Services;
using Endpoint.Application.ValueObjects;

namespace Endpoint.Application.Builders
{
    public class UnitTestBuilder
    {
        private string _name;
        private readonly IContext _context;
        private readonly IFileSystem _fileSystem;
        private string _rootNamespace;
        private string _directory;
        public UnitTestBuilder(IContext context, IFileSystem fileSystem)
        {
            _context = context;
            _fileSystem = fileSystem;
        }

        public UnitTestBuilder WithDirectory(string directory)
        {
            _directory = directory;
            return this;
        }

        public UnitTestBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public UnitTestBuilder WithRootNamespace(string rootNamespace)
        {
            _rootNamespace = rootNamespace;
            return this;
        }

        public void Build()
        {
            new ClassBuilder($"{((Token)_name).PascalCase}Tests", _context, _fileSystem)
                .WithUsing("Xunit")
                .WithNamespace($"{_rootNamespace}.UnitTests")
                .WithDirectory(_directory)
                .Build();
        }
    }
}

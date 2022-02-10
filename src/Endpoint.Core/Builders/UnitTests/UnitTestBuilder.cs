using Endpoint.SharedKernal.Services;
using Endpoint.SharedKernal.ValueObjects;

namespace Endpoint.Core.Builders
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
                .WithMethod(new MethodBuilder().WithName("Constuctor")
                .WithReturnType("void")
                .WithAttribute("[Fact]")
                .WithBody(new() { $"var actual = Create{((Token)_name).PascalCase}();" })
                .Build())
                .WithMethod(new MethodBuilder()
                .WithName($"Create{((Token)_name).PascalCase}")
                .WithReturnType(((Token)_name).PascalCase)
                .WithBody(new() { "return new();" })
                .Build())
                .Build();
        }
    }
}

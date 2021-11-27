using Endpoint.Application.Services;
using Endpoint.Application.ValueObjects;
using System.IO;

namespace Endpoint.Application.Builders
{
    public class EntityConfigurationBuilder
    {
        private IContext _context;
        private IFileSystem _fileSystem;
        private string _entity;
        private string _infrastructureNamespace;
        private string _domainNamespace;
        private string _directory;
        public EntityConfigurationBuilder(string entity, string infrastructureNamespace, string domainNamespace, string directory, IFileSystem fileSystem, IContext context = null)
        {
            _directory = directory;
            _entity = entity;
            _context = context ?? new Context();
            _fileSystem = fileSystem;
            _infrastructureNamespace = infrastructureNamespace;
            _domainNamespace = domainNamespace;
        }

        public string[] Class
        {
            get
            {
                var configureMethod = new MethodBuilder()
                    .WithName("Configure")
                    .WithReturnType("void")
                    .WithBody(new() { "" })
                    .WithParameter(new ParameterBuilder(new TypeBuilder().WithGenericType("EntityTypeBuilder", ((Token)_entity).PascalCase)
                    .Build(), "builder").Build()).Build();

                return new ClassBuilder($"{((Token)_entity).PascalCase}Configuration", _context, _fileSystem)
                    .WithNamespace($"{_infrastructureNamespace}.Data")
                    .WithUsing($"{_domainNamespace}.Models")
                    .WithUsing("Microsoft.EntityFrameworkCore")
                    .WithUsing("Microsoft.EntityFrameworkCore.Metadata.Builders")
                    .WithInterface(new TypeBuilder().WithGenericType("IEntityTypeConfiguration", ((Token)_entity).PascalCase).Build())
                    .WithMethod(configureMethod)
                    .Class;
            }
        }
        public void Build()
        {
            _fileSystem.WriteAllLines($"{_directory}{Path.DirectorySeparatorChar}{((Token)_entity).PascalCase}Configuration.cs", Class);
        }
    }
}

using Endpoint.Application.Builders;
using Endpoint.Application.Models;
using Endpoint.Application.ValueObjects;
using System.IO;
using System.Linq;

namespace Endpoint.Application.Services.FileServices
{
    public class InfrastructureFileService : BaseFileService, IInfrastructureFileService
    {
        public InfrastructureFileService(
            ICommandService commandService,
            ITemplateProcessor templateProcessor,
            ITemplateLocator templateLocator,
            IFileSystem fileSystem)
            : base(commandService, templateProcessor, templateLocator, fileSystem)
        { }

        public void Build(Settings settings)
        {
            _removeDefaultFiles(settings.InfrastructureDirectory);

            _createFolder($"Data", settings.InfrastructureDirectory);

            _buildSeedData(settings);

            var dbContextBuilder = new ClassBuilder(settings.DbContextName, new Context(), _fileSystem)
                .WithDirectory($"{settings.InfrastructureDirectory}{Path.DirectorySeparatorChar}Data")
                .WithUsing($"{settings.ApplicationNamespace}")
                .WithUsing($"{settings.ApplicationNamespace}.Interfaces")
                .WithUsing("Microsoft.EntityFrameworkCore")
                .WithUsing("System.Threading.Tasks")
                .WithUsing("System.Linq")
                .WithNamespace($"{settings.InfrastructureNamespace}.Data")
                .WithInterface($"I{settings.DbContextName}")
                .WithBase("DbContext")
                .WithBaseDependency("DbContextOptions", "options");

            foreach (var resource in settings.Resources.Select(r => (Token)r))
            {
                dbContextBuilder.WithProperty(new PropertyBuilder().WithName(resource.PascalCasePlural).WithType(new TypeBuilder().WithGenericType("DbSet", resource.PascalCase).Build()).WithAccessors(new AccessorsBuilder().WithSetAccessModifuer("private").Build()).Build());

            }

            dbContextBuilder.Build();

        }

        public void BuildAdditionalResource(string additionalResource, Settings settings)
        {
            var dbContextBuilder = new ClassBuilder(settings.DbContextName, new Context(), _fileSystem)
                .WithDirectory($"{settings.InfrastructureDirectory}{Path.DirectorySeparatorChar}Data")
                .WithUsing($"{settings.ApplicationNamespace}")
                .WithUsing($"{settings.ApplicationNamespace}.Interfaces")
                .WithUsing("Microsoft.EntityFrameworkCore")
                .WithUsing("System.Threading.Tasks")
                .WithUsing("System.Linq")
                .WithNamespace($"{settings.InfrastructureNamespace}.Data")
                .WithInterface($"I{settings.DbContextName}")
                .WithBase("DbContext")
                .WithBaseDependency("DbContextOptions", "options");

            foreach (var resource in settings.Resources.Select(r => (Token)r))
            {
                dbContextBuilder.WithProperty(new PropertyBuilder().WithName(resource.PascalCasePlural).WithType(new TypeBuilder().WithGenericType("DbSet", resource.PascalCase).Build()).WithAccessors(new AccessorsBuilder().WithSetAccessModifuer("private").Build()).Build());

            }

            dbContextBuilder.Build();
        }

        private void _buildSeedData(Models.Settings settings)
        {
            var template = _templateLocator.Get(nameof(SeedDataBuilder));

            var tokens = new TokensBuilder()
                .With(nameof(settings.InfrastructureNamespace), (Token)settings.InfrastructureNamespace)
                .With("DbContext", (Token)settings.DbContextName)
                .Build();

            var contents = _templateProcessor.Process(template, tokens);

            _fileSystem.WriteAllLines($@"{settings.InfrastructureDirectory}{Path.DirectorySeparatorChar}Data{Path.DirectorySeparatorChar}SeedData.cs", contents);
        }
    }
}

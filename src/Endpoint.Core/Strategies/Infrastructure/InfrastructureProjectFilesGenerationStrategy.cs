using Endpoint.Core.Builders;
using Endpoint.Core.Enums;
using Endpoint.Core.Models.Options;
using Endpoint.Core.Models.Syntax;
using Endpoint.Core.Strategies.Infrastructure;
using Endpoint.Core.ValueObjects;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace Endpoint.Core.Services
{
    public class InfrastructureProjectFilesGenerationStrategy : BaseProjectFilesGenerationStrategy, IInfrastructureProjectFilesGenerationStrategy
    {
        public InfrastructureProjectFilesGenerationStrategy(
            ICommandService commandService,
            ITemplateProcessor templateProcessor,
            ITemplateLocator templateLocator,
            IFileSystem fileSystem)
            : base(commandService, templateProcessor, templateLocator, fileSystem)
        { }

        public void Build(SettingsModel settings)
        {
            _removeDefaultFiles(settings.InfrastructureDirectory);

            _createFolder($"Data", settings.InfrastructureDirectory);

            _createFolder($"EntityConfigurations", settings.InfrastructureDirectory);

            _buildSeedData(settings);

            _createOrReCreateDbContext(settings);

            _buildEntityConfigurations(settings);
        }

        private void _buildEntityConfigurations(SettingsModel model)
        {
            foreach(var resource in model.Resources)
            {
                _buildEntityConfiguration(model, resource.Name);
            }
        }

        private void _buildEntityConfiguration(SettingsModel model, string resourceName)
        {
            var idPropertyName = model.IdFormat == IdFormat.Short ? "Id" : $"{resourceName}Id";

            new EntityConfigurationFileGenerationStrategy(_templateProcessor, _templateLocator, _fileSystem)
                .Create(resourceName, idPropertyName, model.ApplicationNamespace, model.InfrastructureNamespace, $"{model.InfrastructureDirectory}{Path.DirectorySeparatorChar}EntityConfigurations");
        }

        public void BuildAdditionalResource(string additionalResource, SettingsModel settings)
        {
            _createOrReCreateDbContext(settings);

            _buildEntityConfiguration(settings, additionalResource);
        }

        protected void _createOrReCreateDbContext(SettingsModel settings)
        {
            var dbContextBuilder = new ClassBuilder(settings.DbContextName, new Endpoint.Core.Services.Context(), _fileSystem)
                .WithDirectory($"{settings.InfrastructureDirectory}{Path.DirectorySeparatorChar}Data")
                .WithUsing($"{settings.ApplicationNamespace}")
                .WithUsing($"{settings.ApplicationNamespace}.Interfaces")
                .WithUsing("Microsoft.EntityFrameworkCore")
                .WithUsing("System.Threading.Tasks")
                .WithUsing("System.Linq")
                .WithNamespace($"{settings.InfrastructureNamespace}.Data")
                .WithInterface($"I{settings.DbContextName}")
                .WithBase("DbContext")
                .WithBaseDependency("DbContextOptions", "options")
                    .WithMethod(new MethodBuilder().WithName("OnModelCreating").WithReturnType("void").WithAccessModifier(AccessModifier.Protected).WithOverride().WithParameter("ModelBuilder modelBuilder")
                    .WithBody(new List<string>
                    {
                    "base.OnModelCreating(modelBuilder);",
                    "",
                    $"modelBuilder.ApplyConfigurationsFromAssembly(typeof({((Token)settings.DbContextName).PascalCase}).Assembly);"
                    })
                    .Build());

            foreach (var resource in settings.Resources.Select(r => (Token)r.Name))
            {
                dbContextBuilder.WithProperty(new PropertyBuilder().WithName(resource.PascalCasePlural).WithType(new TypeBuilder().WithGenericType("DbSet", resource.PascalCase).Build()).WithAccessors(new AccessorsBuilder().WithSetAccessModifuer("private").Build()).Build());
            }

            dbContextBuilder.Build();
        }
        private void _buildSeedData(SettingsModel settings)
        {
            var template = _templateLocator.Get("SeedData");

            var tokens = new TokensBuilder()
                .With(nameof(settings.InfrastructureNamespace), (Token)settings.InfrastructureNamespace)
                .With("DbContext", (Token)settings.DbContextName)
                .Build();

            var contents = string.Join(Environment.NewLine,_templateProcessor.Process(template, tokens));

            _fileSystem.WriteAllText($@"{settings.InfrastructureDirectory}{Path.DirectorySeparatorChar}Data{Path.DirectorySeparatorChar}SeedData.cs", contents);
        }
    }
}

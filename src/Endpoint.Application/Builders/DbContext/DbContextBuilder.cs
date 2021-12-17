using Endpoint.Application.Enums;
using Endpoint.Application.Services;
using Endpoint.Application.ValueObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Endpoint.Application.Builders
{
    public class DbContextBuilder : BuilderBase<DbContextBuilder>
    {
        private Token _modelsDirectory;
        private Token _dbContext;
        private List<Token> _models = new List<Token>();
        private IContext _context;

        public DbContextBuilder(
            ICommandService commandService,
            ITemplateProcessor templateProcessor,
            ITemplateLocator templateLocator,
            IFileSystem fileSystem) : base(commandService, templateProcessor, templateLocator, fileSystem)
        { }

        public DbContextBuilder(
            string dbContext,
            IContext context,
            IFileSystem fileSystem,
            string infrastructureDirectory,
            string infrastructureNamespace,
            string domainDirectory,
            string domainNamespace,
            string applicationDirectory,
            string applicationNamespace,
            string[] models
            ) : base(default, default, default, fileSystem)
        {
            _dbContext = (Token)dbContext;
            _context = context;
            _infrastructureDirectory = (Token)infrastructureDirectory;
            _infrastructureNamespace = (Token)infrastructureNamespace;
            _domainDirectory = (Token)domainDirectory;
            _domainNamespace = (Token)domainNamespace;
            _applicationDirectory = (Token)applicationDirectory;
            _applicationNamespace = (Token)applicationNamespace;
            _models = models.Select(x => (Token)x).ToList();
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

        public DbContextBuilder WithDbContextName(string dbContextName)
        {
            _dbContext = (Token)dbContextName;
            return this;
        }

        public void Build()
        {
            try
            {
                var dbContextBuilder = new ClassBuilder(_dbContext.PascalCase, _context, _fileSystem)
                    .WithDirectory($"{_infrastructureDirectory.Value}{Path.DirectorySeparatorChar}Data")
                    .WithUsing($"{_domainNamespace.Value}.Models")
                    .WithUsing($"{_domainNamespace.Value}.Core")
                    .WithUsing($"{_applicationNamespace.Value}.Interfaces")
                    .WithUsing("Microsoft.EntityFrameworkCore")
                    .WithUsing("System.Threading.Tasks")
                    .WithUsing("System.Linq")
                    .WithNamespace($"{_infrastructureNamespace.Value}.Data")
                    .WithInterface($"I{_dbContext.PascalCase}")
                    .WithBase("DbContext")
                    .WithBaseDependency("DbContextOptions", "options");

                var dbContextInterfaceBuilder = new ClassBuilder(_dbContext.PascalCase, _context, _fileSystem, "interface")
                    .WithDirectory($"{_applicationDirectory.Value}{Path.DirectorySeparatorChar}Interfaces")
                    .WithUsing($"{_domainNamespace.Value}.Models")
                    .WithUsing("Microsoft.EntityFrameworkCore")
                    .WithUsing("System.Threading.Tasks")
                    .WithUsing("System.Threading")
                    .WithNamespace($"{_applicationNamespace.Value}.Interfaces")
                    .WithMethodSignature(new MethodSignatureBuilder()
                    .WithAsync(false)
                    .WithAccessModifier(AccessModifier.Inherited)
                    .WithName("SaveChangesAsync")
                    .WithReturnType(new TypeBuilder().WithGenericType("Task", "int").Build())
                    .WithParameter(new ParameterBuilder("CancellationToken", "cancellationToken").Build()).Build());

                foreach (var model in _models)
                {
                    dbContextBuilder.WithProperty(new PropertyBuilder().WithName(model.PascalCasePlural).WithType(new TypeBuilder().WithGenericType("DbSet", model.PascalCase).Build()).WithAccessors(new AccessorsBuilder().WithSetAccessModifuer("private").Build()).Build());

                    dbContextInterfaceBuilder.WithProperty(new PropertyBuilder().WithName(model.PascalCasePlural).WithAccessModifier(AccessModifier.Inherited).WithType(new TypeBuilder().WithGenericType("DbSet", model.PascalCase).Build()).WithAccessors(new AccessorsBuilder().WithGetterOnly().Build()).Build());
                }

                dbContextInterfaceBuilder.Build();

                dbContextBuilder.Build();
            }
            catch (Exception e)
            {
                throw e;
            }


        }
    }
}

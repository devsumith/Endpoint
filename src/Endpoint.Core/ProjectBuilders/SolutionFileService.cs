using Endpoint.SharedKernal;
using Endpoint.SharedKernal.Models;
using Endpoint.SharedKernal.Services;
using Endpoint.SharedKernal.ValueObjects;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using static System.Text.Json.JsonSerializer;

namespace Endpoint.Core.Services
{
    public class SolutionFileService : ISolutionFileService
    {
        protected readonly ICommandService _commandService;
        protected readonly ITemplateProcessor _templateProcessor;
        protected readonly ITemplateLocator _templateLocator;
        protected readonly IFileSystem _fileSystem;

        public SolutionFileService(
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

        public Endpoint.SharedKernal.Models.Settings Build(string name, string properties, string dbContextName, bool useShortIdProperty, bool useIntIdPropertyType, string resource, string directory, bool isMicroserviceArchitecture, List<string> plugins)
        {
            AggregateRoot aggregateRoot = new AggregateRoot(resource);

            aggregateRoot.Properties.Add(new ClassProperty("public", "Guid", $"{((Token)resource).PascalCase}Id", ClassPropertyAccessor.GetPrivateSet, key: true));

            if (!string.IsNullOrWhiteSpace(properties))
            {
                foreach (var property in properties.Split(','))
                {
                    var nameValuePair = property.Split(':');

                    aggregateRoot.Properties.Add(new ClassProperty("public", nameValuePair.ElementAt(1), nameValuePair.ElementAt(0), ClassPropertyAccessor.GetPrivateSet));
                }
            }

            return Build(name, dbContextName, useShortIdProperty, useIntIdPropertyType, new List<AggregateRoot>() { aggregateRoot }, directory, isMicroserviceArchitecture, plugins);
        }

        public Endpoint.SharedKernal.Models.Settings Build(string name, string properties, string dbContextName, bool useShortIdProperty, bool useIntIdPropertyType, List<string> resources, string directory, bool isMicroserviceArchitecture, List<string> plugins)
        {
            var aggregates = new List<AggregateRoot>();

            foreach (var resource in resources)
            {
                AggregateRoot aggregateRoot = new AggregateRoot(resource);

                aggregateRoot.Properties.Add(new ClassProperty("public", "Guid", $"{((Token)resource).PascalCase}Id", ClassPropertyAccessor.GetPrivateSet, key: true));

                if (!string.IsNullOrWhiteSpace(properties))
                {
                    foreach (var property in properties.Split(','))
                    {
                        var nameValuePair = property.Split(':');

                        aggregateRoot.Properties.Add(new ClassProperty("public", nameValuePair.ElementAt(1), nameValuePair.ElementAt(0), ClassPropertyAccessor.GetPrivateSet));
                    }
                }

                aggregates.Add(aggregateRoot);
            }


            return Build(name, dbContextName, useShortIdProperty, useIntIdPropertyType, aggregates, directory, isMicroserviceArchitecture, plugins);

        }

        public Endpoint.SharedKernal.Models.Settings Build(string name, string dbContextName, bool useShortIdProperty, bool useIntIdPropertyType, List<AggregateRoot> resources, string directory, bool isMicroserviceArchitecture, List<string> plugins)
        {

            name = name.Replace("-", "_");

            _commandService.Start($"mkdir {name}", directory);

            var settings = new Endpoint.SharedKernal.Models.Settings(name, dbContextName, resources, directory, isMicroserviceArchitecture, plugins);

            var json = Serialize(settings, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });

            _fileSystem.WriteAllLines($"{settings.RootDirectory}{Path.DirectorySeparatorChar}{SharedKernal.Constants.SettingsFileName}", new List<string> { json }.ToArray());

            _commandService.Start($"dotnet new sln -n {settings.SolutionName}", settings.RootDirectory);

            _commandService.Start($"mkdir {settings.SourceFolder}", settings.RootDirectory);

            _commandService.Start($"mkdir {settings.TestFolder}", settings.RootDirectory);

            _commandService.Start("git init", settings.RootDirectory);

            if (isMicroserviceArchitecture)
            {
                _createProjectAndAddToSolution(Constants.DotNetTemplateTypes.WebApi, settings.ApiDirectory, settings);

                _createProjectAndAddToSolution(Constants.DotNetTemplateTypes.ClassLibrary, settings.TestingDirectory, settings);

                _createProjectAndAddToSolution(Constants.DotNetTemplateTypes.XUnit, settings.UnitTestsDirectory, settings);

                _addReference(settings.TestingDirectory, settings.ApiDirectory);

                _addReference(settings.UnitTestsDirectory, settings.TestingDirectory);
            }
            else
            {
                _createProjectAndAddToSolution(Constants.DotNetTemplateTypes.WebApi, settings.ApiDirectory, settings);

                _createProjectAndAddToSolution(Constants.DotNetTemplateTypes.ClassLibrary, settings.DomainDirectory, settings);

                _createProjectAndAddToSolution(Constants.DotNetTemplateTypes.ClassLibrary, settings.ApplicationDirectory, settings);

                _createProjectAndAddToSolution(Constants.DotNetTemplateTypes.ClassLibrary, settings.InfrastructureDirectory, settings);

                _createProjectAndAddToSolution(Constants.DotNetTemplateTypes.ClassLibrary, settings.TestingDirectory, settings);

                _createProjectAndAddToSolution(Constants.DotNetTemplateTypes.XUnit, settings.UnitTestsDirectory, settings);

                _addReference(settings.ApplicationDirectory, settings.DomainDirectory);

                _addReference(settings.InfrastructureDirectory, settings.ApplicationDirectory);

                _addReference(settings.ApiDirectory, settings.InfrastructureDirectory);

                _addReference(settings.TestingDirectory, settings.ApiDirectory);

                _addReference(settings.TestingDirectory, settings.ApiDirectory);

                _addReference(settings.UnitTestsDirectory, settings.TestingDirectory);
            }

            return settings;
        }
        private void _createProjectAndAddToSolution(string templateType, string directory, Endpoint.SharedKernal.Models.Settings settings)
        {
            _commandService.Start($@"mkdir {directory}");

            _commandService.Start($"dotnet new {templateType} --framework net5.0", directory);

            var parts = directory.Split(Path.DirectorySeparatorChar);

            var name = parts[parts.Length - 1];

            _commandService.Start($"dotnet sln add {directory}{Path.DirectorySeparatorChar}{name}.csproj", settings.RootDirectory);
        }

        private void _addReference(string targetDirectory, string referencedDirectory)
        {
            var parts = referencedDirectory.Split(Path.DirectorySeparatorChar);

            var name = parts[parts.Length - 1];

            _commandService.Start($"dotnet add {targetDirectory} reference {referencedDirectory}{Path.DirectorySeparatorChar}{name}.csproj");
        }
    }
}

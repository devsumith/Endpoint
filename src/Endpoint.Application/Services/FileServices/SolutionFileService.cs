using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using static System.Text.Json.JsonSerializer;

namespace Endpoint.Application.Services.FileServices
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

        public Models.Settings Build(string name, string resource, string directory, bool isMicroserviceArchitecture)
        {
            name = name.Replace("-", "_");

            _commandService.Start($"mkdir {name}", directory);

            var settings = new Models.Settings(name, resource, directory, isMicroserviceArchitecture);

            var json =  Serialize(settings, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });

            _fileSystem.WriteAllLines($"{settings.RootDirectory}{Path.DirectorySeparatorChar}{Constants.SettingsFileName}", new List<string> { json }.ToArray());

            _commandService.Start($"dotnet new sln -n {settings.SolutionName}", settings.RootDirectory);

            _commandService.Start($"mkdir {settings.SourceFolder}", settings.RootDirectory);

            _commandService.Start($"mkdir {settings.TestFolder}", settings.RootDirectory);

            _commandService.Start("git init", settings.RootDirectory);

            if(isMicroserviceArchitecture)
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

        private void _createProjectAndAddToSolution(string templateType, string directory, Models.Settings settings)
        {
            _commandService.Start($@"mkdir {directory}");

            _commandService.Start($"dotnet new {templateType} --framework net5.0", directory);

            var parts = directory.Split(Path.DirectorySeparatorChar);

            var name = parts[parts.Length -1];

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

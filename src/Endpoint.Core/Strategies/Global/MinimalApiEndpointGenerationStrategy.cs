using Endpoint.Core.Models;
using Endpoint.Core.Services;
using System;
using Endpoint.Core.Models;
using Endpoint.Core.Strategies.Global;
using Endpoint.Core.ValueObjects;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using static System.Text.Json.JsonSerializer;
using Endpoint.Core.Utilities;
using Endpoint.Core.Strategies.Api.FileGeneration;

namespace Endpoint.Core.Strategies.Global
{
    internal class MinimalApiEndpointGenerationStrategy : IEndpointGenerationStrategy
    {
        private readonly ICommandService _commandService;
        private readonly IFileSystem _fileSystem;
        private readonly ITemplateLocator _templateLocator;
        private readonly ITemplateProcessor _templateProcessor;
        private readonly ILaunchSettingsGenerationStrategy _launchSettingsGenerationStrategy;


        public MinimalApiEndpointGenerationStrategy(ICommandService commandService, IFileSystem fileSystem, ITemplateLocator templateLocator, ITemplateProcessor templateProcessor)
        {
            _commandService = commandService;
            _fileSystem = fileSystem;
            _templateLocator = templateLocator;
            _templateProcessor = templateProcessor;
            _launchSettingsGenerationStrategy = new LaunchSettingsGenerationStrategy(templateProcessor, fileSystem, templateLocator);
        }

        public bool CanHandle(Settings model)
        {
            return model.Minimal;
        }

        public void Create(Settings model)
        {
            var json = Serialize(model, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });

            _commandService.Start($"mkdir {model.RootDirectory}");

            _fileSystem.WriteAllLines($"{model.RootDirectory}{Path.DirectorySeparatorChar}{CoreConstants.SettingsFileName}", new List<string> { json }.ToArray());

            _commandService.Start($"dotnet new sln -n {model.SolutionName}", model.RootDirectory);

            _commandService.Start($"mkdir {model.SourceFolder}", model.RootDirectory);

            _commandService.Start($"mkdir {model.TestFolder}", model.RootDirectory);

            _commandService.Start($"mkdir deploy", model.RootDirectory);

            _commandService.Start("git init", model.RootDirectory);

            new GitIgnoreFileGenerationStrategy(_fileSystem, _templateLocator).Generate(model);

            _commandService.Start($@"mkdir {model.ApiDirectory}");

            _commandService.Start($"dotnet new webapi --framework net6.0", model.ApiDirectory);

            var parts = model.ApiDirectory.Split(Path.DirectorySeparatorChar);

            var name = parts[parts.Length - 1];

            _commandService.Start($"dotnet sln add {model.ApiDirectory}{Path.DirectorySeparatorChar}{name}.csproj", model.RootDirectory);

            _commandService.Start($"rimraf WeatherForecast.cs", $@"{model.ApiDirectory}");

            _commandService.Start($@"rimraf Controllers\WeatherForecastController.cs", $@"{model.ApiDirectory}");

            _commandService.Start($"start {model.SolutionFileName}", model.RootDirectory);

            new BicepFileGenerationStrategy(_fileSystem, _templateLocator).Generate(model);

            new DeploySetupFileGenerationStrategy(_fileSystem, _templateLocator, _templateProcessor).Generate(model);

            var csProjFilePath = $"{model.ApiDirectory}{Path.DirectorySeparatorChar}{model.ApiNamespace}.csproj";

            CsProjectUtilities.AddGenerateDocumentationFile(csProjFilePath);

            CsProjectUtilities.RemoveDefaultWebApiFiles(model.ApiDirectory);

            _launchSettingsGenerationStrategy.Create(model);

        }
    }
}

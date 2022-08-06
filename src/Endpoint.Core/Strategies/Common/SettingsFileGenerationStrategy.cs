using Endpoint.Core.Models;
using Endpoint.Core.Services;
using System.IO;
using System.Text.Json;
using static System.Text.Json.JsonSerializer;

namespace Endpoint.Core.Strategies
{
    public class SettingsFileGenerationStrategy : ISettingsFileGenerationStrategy
    {
        private readonly IFileSystem _fileSystem;

        public SettingsFileGenerationStrategy(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public bool? CanHandle(Settings request) => !request.Minimal;
        public Settings Create(Settings model)
        {
            var json = Serialize(model, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });

            _fileSystem.WriteAllLines($"{model.Directory}{Path.DirectorySeparatorChar}cliSettings.json", new string[1] { json });

            return new();
        }
    }

    public class MinimalApiSettingsFileGenerationStrategy : ISettingsFileGenerationStrategy
    {
        public bool? CanHandle(Settings request) => request.Minimal;

        public Settings Create(Settings request)
        {

            return new();
        }
    }
}

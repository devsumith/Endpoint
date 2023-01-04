using Endpoint.Core.Factories;
using Endpoint.Core.Models.Options;
using Endpoint.Core.Services;
using System;
using System.IO;
using System.Text.Json;
using static System.Text.Json.JsonSerializer;

namespace Endpoint.Core.Strategies.Solutions.Update
{
    public class SettingsUpdateStrategy : ISettingsUpdateStrategy
    {
        private readonly IFileSystem _fileSystem;

        public SettingsUpdateStrategy(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        }

        public bool CanHandle(SolutionSettingsModel model, string entityName, string properties)
        {
            return true;
        }

        public void Update(SolutionSettingsModel model, string entityName, string properties)
        {
            model.Entities.Add(EntityFactory.Create(entityName, properties));

            var json = Serialize(model, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });

            _fileSystem.WriteAllLines($"{model.Directory}{Path.DirectorySeparatorChar}cliSettings.json", new string[1] { json });
        }

        public int Order => 0;
    }
}

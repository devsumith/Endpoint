using Endpoint.Core.Models;
using Endpoint.Core.Services;
using System;
using System.IO;
using System.Text.Json;
using static System.Text.Json.JsonSerializer;

namespace Endpoint.Core.Strategies
{
    public class SettingsFileGenerationStrategy : ISolutionSettingsFileGenerationStrategy
    {
        private readonly IFileSystem _fileSystem;
        private readonly INamingConventionConverter _namingConventionConverter;

        public SettingsFileGenerationStrategy(IFileSystem fileSystem, INamingConventionConverter namingConventionConverter)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(namingConventionConverter));
            _namingConventionConverter = namingConventionConverter ?? throw new System.ArgumentNullException(nameof(namingConventionConverter));
        }

        public bool? CanHandle(SolutionSettingsModel request) => !request.Metadata.Contains(CoreConstants.SolutionTemplates.Minimal);
        public SolutionSettingsModel Create(SolutionSettingsModel model)
        {
            var json = Serialize(model, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });

            _fileSystem.WriteAllLines($"{model.Directory}{Path.DirectorySeparatorChar}cliSettings.${_namingConventionConverter.Convert(NamingConvention.CamelCase,model.Namespace)}.json", new string[1] { json });

            return model;
        }
    }
}

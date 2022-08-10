using Endpoint.Core.Models;
using Endpoint.Core.Services;
using Endpoint.Core.Strategies.Solutions.Crerate;
using System.IO;
using System.Text.Json;

namespace Endpoint.Core.Strategies
{
    public class WorkspaceGenerationStrategy : IWorkspaceSettingsGenerationStrategy
    {
        private readonly IFileSystem _fileSystem;

        public WorkspaceGenerationStrategy(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public int Order { get; set; } = 0;
        public bool CanHandle(WorkspaceSettingsModel model) => true;
        public void Create(WorkspaceSettingsModel model)
        {
            _fileSystem.CreateDirectory($"{model.Directory}");

            var json = JsonSerializer.Serialize(model, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });

            _fileSystem.WriteAllLines($"{model.Directory}{Path.DirectorySeparatorChar}Workspace.json", new string[1] { json });
        }
    }
}

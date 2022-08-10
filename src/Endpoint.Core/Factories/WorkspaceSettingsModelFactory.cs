using Endpoint.Core.Models;
using Endpoint.Core.Options;
using System.IO;
using static System.Text.Json.JsonSerializer;
using static System.IO.File;

namespace Endpoint.Core.Factories
{
    public static class WorkspaceSettingsModelFactory
    {
        public static WorkspaceSettingsModel Create(ResolveOrCreateWorkspaceOptions options)
        {
            var model = new WorkspaceSettingsModel
            {
                Directory = $"{options.Directory}{Path.DirectorySeparatorChar}{options.Name}",
                Name = options.Name
            };

            return model;
        }

        public static WorkspaceSettingsModel Resolve(ResolveOrCreateWorkspaceOptions options)
        {
            var json = ReadAllText($"{options.Directory}{Path.DirectorySeparatorChar}{options.Name}{Path.DirectorySeparatorChar}Workspace.json");

            return Deserialize<WorkspaceSettingsModel>(json);
        }
    }
}

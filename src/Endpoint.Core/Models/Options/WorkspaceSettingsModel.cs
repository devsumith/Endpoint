using System.Collections.Generic;

namespace Endpoint.Core.Models.Options
{
    public class WorkspaceSettingsModel
    {
        public string Directory { get; set; }
        public string Name { get; set; }
        public List<SolutionSettingsModel> SolutionSettings { get; set; } = new List<SolutionSettingsModel> { };
    }
}

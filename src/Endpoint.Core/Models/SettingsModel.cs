using System.Collections.Generic;

namespace Endpoint.Core.Models
{
    public class SolutionSettingsModel
    {
        public SolutionSettingsModel()
        {

        }
        public string Path => $"{Directory}{System.IO.Path.DirectorySeparatorChar}CliSettings.{Namespace}.json";
        public string Namespace { get; set; }
        public List<Entity> Entities { get; set; } = new List<Entity>();
        public string Directory { get; set; }
        public List<string> Metadata { get; set; } = new List<string>();
    }

    public class SettingsModel
    {
        public List<SolutionSettingsModel> Settings { get; set; }
    }
}

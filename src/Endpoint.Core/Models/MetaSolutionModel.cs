using System.Collections.Generic;

namespace Endpoint.Core.Models
{
    public class MetaSolutionSettingsModel
    {
        public string Directory { get; set; }
        public string Name { get; set; }
        public List<SolutionSettingsModel> SolutionSettings { get; set; } = new List<SolutionSettingsModel> { };
    }
}

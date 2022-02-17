using System.Collections.Generic;

namespace Endpoint.Core.Models
{
    public class NamespaceModel
    {
        public List<string> Usings { get; set; }
        public string Name { get; set; }
        public List<ClassModel> Classes { get; set; }
    }
}

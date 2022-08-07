using System.Collections.Generic;

namespace Endpoint.Core.Models.Files
{
    public class TemplatedFileModel : FileModel
    {
        public string Template { get; init; }
        public Dictionary<string, object> Tokens { get; init; } = new();

        public TemplatedFileModel()
        {

        }
    }
}

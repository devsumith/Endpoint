using System.Collections.Generic;

namespace Endpoint.Core.Models.Artifacts.Files
{

    public class CSharpTemplatedFileModel : TemplatedFileModel
    {
        public CSharpTemplatedFileModel(string templateName, string name, string directory, string @namespace, Dictionary<string, object> tokens = null) 
            : base(templateName, name, directory, "cs", tokens)
        {
            Namespace = @namespace;
        }

        public string Namespace { get; init; }
    }
}

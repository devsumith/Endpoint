using System.Collections.Generic;

namespace Endpoint.Core.Models.Artifacts.Files;

public class TemplatedFileModel : FileModel
{
    public TemplatedFileModel(string templateName, string name, string directory, string extension, Dictionary<string, object> tokens = null)
        : base(name, directory, extension)
    {
        Tokens = new Dictionary<string, object>();
        Template = templateName;

        if (tokens != null)
        {
            foreach (var token in tokens) {

                Tokens.TryAdd(token.Key, token.Value); 
            }
        }
    }

    public string Template { get; init; }
    public Dictionary<string, object> Tokens { get; init; } = new();

}

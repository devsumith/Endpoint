namespace Endpoint.Core.Models.Artifacts.Files;

public class ContentFileModel : FileModel
{
    public ContentFileModel(string content, string name, string directory, string extension, string copyright = null) 
        :base(name, directory, extension, copyright)
    {
        Content = content;
    }
    
    public string Content { get; init; }
}

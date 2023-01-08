using Endpoint.Core.Models.Syntax;
using System.Collections.Generic;

namespace Endpoint.Core.Models.Artifacts.Files;

public class ObjectFileModel<T> : FileModel
{
    public ObjectFileModel(T @object, List<UsingDirectiveModel> usings, string name, string directory, string extension)
        :base(name,directory, extension)
    {
        Object = @object;
        Usings = usings;
    }

    public T Object { get; init; }
    public List<UsingDirectiveModel> Usings { get; set; } = new List<UsingDirectiveModel>();
}

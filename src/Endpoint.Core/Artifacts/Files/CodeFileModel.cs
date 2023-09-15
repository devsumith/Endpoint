// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core.Artifacts.Files;

public class CodeFileModel<T> : FileModel
    where T : SyntaxModel
{
    public CodeFileModel()
    {

    }

    public CodeFileModel(T @object, string name, string directory, string extension)
    : base(name.Split('.').Last(), directory, extension)
    {
        Object = @object;
        Usings = @object.Usings;
    }

    public CodeFileModel(T @object, List<UsingModel> usings, string name, string directory, string extension)
        : base(name.Split('.').Last(), directory, extension)
    {
        Object = @object;
        Usings = usings;
    }

    public T Object { get; set; }

    public List<UsingModel> Usings { get; set; }

    public string Namespace { get; set; }
}


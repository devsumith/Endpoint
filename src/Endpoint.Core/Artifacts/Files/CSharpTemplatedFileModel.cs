// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.


using System.Collections.Generic;


namespace Endpoint.Core.Artifacts.Files;


public class CSharpTemplatedFileModel : TemplatedFileModel
{
    public CSharpTemplatedFileModel(string templateName, string name, string directory, string @namespace, Dictionary<string, object> tokens = null)
        : base(templateName, name, directory, ".cs", tokens)
    {
        Namespace = @namespace;
    }

    public string Namespace { get; init; }
}



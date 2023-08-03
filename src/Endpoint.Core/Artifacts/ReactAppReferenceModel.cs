// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.Artifacts;

public class ReactAppReferenceModel
{
    public ReactAppReferenceModel(string name, string referenceDirectory)
    {
        Name = name;
        ReferenceDirectory = referenceDirectory;
    }

    public string Name { get; set; }
    public string ReferenceDirectory { get; set; }
}


// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.IO;

namespace Endpoint.Core.Services;

public class SolutionNamespaceProvider : ISolutionNamespaceProvider
{
    private readonly IFileProvider fileProvider;

    public SolutionNamespaceProvider(IFileProvider fileProvider)
    {
        this.fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
    }

    public string Get(string directory)
    {
        if (!Directory.Exists(directory))
        {
            return "SolutionNamespaceNotFound";
        }

        return Path.GetFileNameWithoutExtension(fileProvider.Get("*.sln", directory));
    }
}

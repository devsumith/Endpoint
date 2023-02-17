// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.IO;

namespace Endpoint.Core.Services;

public class SolutionNamespaceProvider : ISolutionNamespaceProvider
{
    private readonly IFileProvider _fileProvider;

    public SolutionNamespaceProvider(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
    }
    public string Get(string directory)
    {
        if (!Directory.Exists(directory))
        {
            return "SolutionNamespaceNotFound";
        }

        return Path.GetFileNameWithoutExtension(_fileProvider.Get("*.sln", directory));
    }
}


// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.IO;
using System.Linq;

namespace Endpoint.Core.Services;

public class FileNamespaceProvider : IFileNamespaceProvider
{
    private readonly IFileProvider fileProvider;

    public FileNamespaceProvider(IFileProvider fileProvider)
    {
        this.fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
    }

    public string Get(string directory)
    {
        var projectNamespace = Path.GetFileNameWithoutExtension(fileProvider.Get("*.csproj", directory));

        var projectDirectoryParts = Path.GetDirectoryName(fileProvider.Get("*.csproj", directory)).Split(Path.DirectorySeparatorChar);

        var fileDirectoryParts = directory.Split(Path.DirectorySeparatorChar).Skip(projectDirectoryParts.Length);

        return fileDirectoryParts.Any() ? $"{projectNamespace}.{string.Join(".", fileDirectoryParts)}" : projectNamespace;
    }
}

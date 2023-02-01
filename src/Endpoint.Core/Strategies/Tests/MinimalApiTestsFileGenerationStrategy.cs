// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Models.Artifacts.Files;
using Endpoint.Core.Services;
using System.Collections.Generic;
using System.IO;

namespace Endpoint.Core.Strategies.Tests;

public interface IMinimalApiTestsFileGenerationStrategy
{
    void Create(MinimalApiProgramFileModel model, string directory);
}

public class MinimalApiTestsFileGenerationStrategy : IMinimalApiTestsFileGenerationStrategy
{
    private readonly IFileSystem _fileSystem;
    public MinimalApiTestsFileGenerationStrategy(IFileSystem fileSystem)
    {
        _fileSystem = fileSystem ?? throw new System.ArgumentNullException(nameof(fileSystem)); 
    }

    public void Create(MinimalApiProgramFileModel model, string directory)
    {
        var content = new List<string>();
        
        _fileSystem.WriteAllText($"{directory}{Path.DirectorySeparatorChar}Tests.cs", string.Join(Environment.NewLine, content));
    }
}


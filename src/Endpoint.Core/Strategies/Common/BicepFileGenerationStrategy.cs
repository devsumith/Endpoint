// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Options;
using Endpoint.Core.Services;
using System.IO;

namespace Endpoint.Core.Strategies.Common;

public interface IBicepFileGenerationStrategy
{
    void Generate(SettingsModel settings);
}

public class BicepFileGenerationStrategy : IBicepFileGenerationStrategy
{
    private readonly IFileSystem _fileSystem;
    private readonly ITemplateLocator _templateLocator;

    public BicepFileGenerationStrategy(IFileSystem fileSystem, ITemplateLocator templateLocator)
    {
        _fileSystem = fileSystem;
        _templateLocator = templateLocator;
    }
    public void Generate(SettingsModel settings)
    {
        _fileSystem.WriteAllText($"{settings.RootDirectory}{Path.DirectorySeparatorChar}deploy{Path.DirectorySeparatorChar}main.bicep", string.Join(Environment.NewLine, _templateLocator.Get("BicepFile")));
    }
}


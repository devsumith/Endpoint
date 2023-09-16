// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;

namespace Endpoint.Core.Artifacts.Files.Strategies;

public class DbContextFileFromCoreDirectoryArtifactGenerationStrategy : GenericArtifactGenerationStrategy<string>
{
    private readonly IFileProvider _fileProvider;

    public DbContextFileFromCoreDirectoryArtifactGenerationStrategy(IServiceProvider serviceProvider, IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }



    public bool CanHandle(object model)
    {
        if (model is string value)
        {
            var projectDirectory = _fileProvider.Get("*.csproj", value);
        }

        return false;
    }

    public override async Task GenerateAsync(IArtifactGenerator artifactGenerator, string directory)
    {
        throw new NotImplementedException();
    }
}

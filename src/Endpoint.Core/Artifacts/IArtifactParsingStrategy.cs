// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.Artifacts;

public interface IArtifactParsingStrategy
{
    int GetPriority();

    Task<object> ParseObjectAsync(IArtifactParser parser, string valueOrDirectoryOrPath);
}

public interface IArtifactParsingStrategy<T> : IArtifactParsingStrategy
    where T : class
{
    

    Task<T> ParseAsync(IArtifactParser parser, string valueOrDirectoryOrPath);
}

// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.Artifacts;

public abstract class BaseArtifactParsingStrategy<T> : IArtifactParsingStrategy<T>
    where T : class
{
    public virtual int GetPriority() { 
    
        return 0;
    } 

    public abstract Task<T> ParseAsync(IArtifactParser parser, string valueOrDirectoryOrPath);

    public async Task<object> ParseObjectAsync(IArtifactParser parser, string valueOrDirectoryOrPath)
        => await ParseAsync(parser, valueOrDirectoryOrPath);
}

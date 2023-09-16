// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.Artifacts;

public abstract class GenericArtifactGenerationStrategy<T> : IGenericArtifactGenerationStrategy<T>
    where T : class
{
    public abstract Task GenerateAsync(IArtifactGenerator generator, T model);

    public virtual async Task<bool> GenerateAsync(IArtifactGenerator generator, object target)
    {
        if (target is T model)
        {
            await GenerateAsync(generator, model);

            return true;
        }
        else
            return false;
    }

    public virtual int GetPriority() => 0;
}
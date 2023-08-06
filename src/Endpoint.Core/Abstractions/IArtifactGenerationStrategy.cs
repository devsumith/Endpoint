// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.Abstractions;

public interface IArtifactGenerationStrategy
{
    bool CanHandle(object model, dynamic context = null);
    Task CreateAsync(object model, dynamic context = null);
    int Priority { get; }
}

public interface IArtifactGenerationStrategy<T>
{
    bool CanHandle(T model);
    Task GenerateAsync(IArtifactGenerator generator, T model, dynamic context = null);
    int Priority { get; }
}

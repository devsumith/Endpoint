// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Syntax.Entities.Aggregate;

namespace Endpoint.Core.Abstractions;

public interface IArtifactGenerationStrategy<T>
{
    public bool CanHandle(T model, dynamic context = null) => model is T;
    Task GenerateAsync(IArtifactGenerator generator, T model, dynamic context = null);
    int Priority { get; }


}

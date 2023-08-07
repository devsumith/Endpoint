// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.Abstractions;

public interface ISyntaxGenerationStrategy<T>
{
    public bool CanHandle(T model, dynamic context = null) => model is T;

    Task<string> GenerateAsync(ISyntaxGenerator generator, T model, dynamic context = null);
    
    int Priority { get; }
}


// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.Syntax;

public interface ISyntaxGenerationStrategy
{
    Task<string> GenerateAsync(ISyntaxGenerator generator, object target, dynamic context = null);

    int GetPriority();
}


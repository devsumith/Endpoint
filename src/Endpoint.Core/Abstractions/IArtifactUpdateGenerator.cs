// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.Abstractions;

public interface IArtifactUpdateGenerator
{
    void CreateFor(dynamic context = null, params object[] args);
}

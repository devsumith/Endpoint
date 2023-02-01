// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.Abstractions;

public interface IArtifactUpdateStrategy
{
    int Priority { get; }
    bool CanHandle(dynamic context = null, params object[] args);

    void Update(dynamic context = null, params object[] args);
}

// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Models.WebArtifacts;

namespace Endpoint.Core.Abstractions;

public interface IWebGenerationStrategy
{
    bool CanHandle(LitWorkspaceModel model, dynamic context = null);
    void Create(LitWorkspaceModel model, dynamic context = null);
    int Priority { get; }
}


// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Models.WebArtifacts;

namespace Endpoint.Core.Abstractions;

public interface IWebGenerationStrategyFactory
{
    void CreateFor(LitWorkspaceModel model, dynamic context = null);
}


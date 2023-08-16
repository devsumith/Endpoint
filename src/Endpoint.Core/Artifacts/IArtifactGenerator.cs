// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.Artifacts;

public interface IArtifactGenerator
{
    Task GenerateAsync(object model, dynamic context = null);
}

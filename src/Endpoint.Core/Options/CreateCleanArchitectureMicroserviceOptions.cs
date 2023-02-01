// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.Options;

public class CreateCleanArchitectureMicroserviceOptions
{
    public string Name { get; set; } = string.Empty;
    public string Directory { get; set; } = string.Empty;
    public string SolutionDirectory { get; set; } = string.Empty;

}

public class ResolveOrCreateWorkspaceOptions
{
    public string Directory { get; set; } = string.Empty;
    public string Name { get; set; }
}


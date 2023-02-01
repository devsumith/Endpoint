// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Options;

namespace Endpoint.Core.Models.Artifacts.Solutions;

public interface ISolutionModelFactory
{
    SolutionModel Create(string name);
    SolutionModel Create(string name, string projectName, string dotNetProjectTypeName, string folderName, string directory);
    SolutionModel Minimal(CreateEndpointSolutionOptions options);
    SolutionModel CreateHttpSolution(CreateEndpointSolutionOptions options);
    SolutionModel CleanArchitectureMicroservice(CreateCleanArchitectureMicroserviceOptions options);
}

// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Options;

namespace Endpoint.Core.Artifacts.Solutions.Factories;

public interface ISolutionFactory
{
    Task<SolutionModel> Create(string name);

    Task<SolutionModel> Create(string name, string projectName, string dotNetProjectTypeName, string folderName, string directory);

    Task<SolutionModel> Minimal(CreateEndpointSolutionOptions options);

    Task<SolutionModel> CreateHttpSolution(CreateEndpointSolutionOptions options);

    Task<SolutionModel> CleanArchitectureMicroservice(CreateCleanArchitectureMicroserviceOptions options);
}

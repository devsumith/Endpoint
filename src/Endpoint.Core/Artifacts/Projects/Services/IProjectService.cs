// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.Artifacts.Projects.Services;

public interface IProjectService
{
    void AddProject(ProjectModel model);
    void AddToSolution(ProjectModel model);
    void AddGenerateDocumentationFile(string csprojFilePath);
    void AddEndpointPostBuildTargetElement(string csprojFilePath);
    void PackageAdd(string name, string directory);
    void CoreFilesAdd(string directory);
    void CorePackagesAdd(string directory);
    void CorePackagesAndFiles(string directory);

}


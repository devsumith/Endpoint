// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Options;
using System.Collections.Generic;

namespace Endpoint.Core.Models.Artifacts.Projects.Factories;

public interface IProjectModelFactory
{
    ProjectModel CreateSpecFlowProject(string name, string directory);
    ProjectModel CreateHttpProject(string name, string directory);
    ProjectModel CreateMinimalApiProject(CreateMinimalApiProjectOptions options);
    ProjectModel CreateMinimalApiUnitTestsProject(string name, string directory, string resource);
    ProjectModel CreateLibrary(string name, string parentDirectory, List<string> additionalMetadata = null);
    ProjectModel CreateWebApi(string name, string parentDirectory, List<string> additionalMetadata = null);
    ProjectModel CreateTestingProject();
    ProjectModel CreateUnitTestsProject();
    ProjectModel CreateIntegrationTestsProject();
    ProjectModel CreateMessagingProject(string directory);
    ProjectModel CreateMessagingUdpProject(string directory);
    ProjectModel CreateSecurityProject(string directory);
    ProjectModel CreateKernelProject(string directory);
    ProjectModel CreatePlaywrightProject(string name, string directory);
    ProjectModel Create(string type, string name, string directory, List<string> references = null, string metadata = null);
    ProjectModel CreateCore(string name, string directory);
    ProjectModel CreateInfrastructure(string name, string directory);
    ProjectModel CreateApi(string name, string directory);
    ProjectModel CreateKernal(string directory);

}


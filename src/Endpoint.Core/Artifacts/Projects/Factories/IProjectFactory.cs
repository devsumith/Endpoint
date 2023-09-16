// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using Endpoint.Core.Options;

namespace Endpoint.Core.Artifacts.Projects.Factories;

public interface IProjectFactory
{
    Task<ProjectModel> CreateSpecFlowProject(string name, string directory);

    Task<ProjectModel> CreateHttpProject(string name, string directory);

    Task<ProjectModel> CreateMinimalApiProject(CreateMinimalApiProjectOptions options);

    Task<ProjectModel> CreateMinimalApiUnitTestsProject(string name, string directory, string resource);

    Task<ProjectModel> CreateLibrary(string name, string parentDirectory, List<string> additionalMetadata = null);

    Task<ProjectModel> CreateWebApi(string name, string parentDirectory, List<string> additionalMetadata = null);

    Task<ProjectModel> CreateTestingProject();

    Task<ProjectModel> CreateUnitTestsProject();

    Task<ProjectModel> CreateIntegrationTestsProject();

    Task<ProjectModel> CreateMessagingProject(string directory);

    Task<ProjectModel> CreateMessagingUdpProject(string directory);

    Task<ProjectModel> CreateIOCompression(string directory);

    Task<ProjectModel> CreateSecurityProject(string directory);

    Task<ProjectModel> CreateKernelProject(string directory);

    Task<ProjectModel> CreatePlaywrightProject(string name, string directory);

    Task<ProjectModel> Create(string type, string name, string directory, List<string> references = null, string metadata = null);

    Task<ProjectModel> CreateCore(string name, string directory);

    Task<ProjectModel> CreateCommon(string directory);

    Task<ProjectModel> CreateInfrastructure(string name, string directory);

    Task<ProjectModel> CreateApi(string name, string directory);

    Task<ProjectModel> CreateValidationProject(string directory);
}

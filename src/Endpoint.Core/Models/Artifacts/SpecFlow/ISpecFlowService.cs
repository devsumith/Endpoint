// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.Models.Artifacts.SpecFlow;

public interface ISpecFlowService
{
    void CreateHook(string name, string directory);
    void CreatePageObject(string name, string directory);
    void CreateDockerControllerHooks(string directory);

}



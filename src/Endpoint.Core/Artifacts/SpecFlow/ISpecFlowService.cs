// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Threading.Tasks;

namespace Endpoint.Core.Artifacts.SpecFlow;

public interface ISpecFlowService
{
    Task CreateHook(string name, string directory);

    Task CreatePageObject(string name, string directory);

    Task CreateDockerControllerHooks(string directory);
}

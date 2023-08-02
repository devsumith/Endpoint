// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.WebArtifacts.Factories;

public interface IWebArtifactModelsFactory
{
    AngularProjectModel Create(string name, string prefix, string directory);
}


// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.Artifacts.Files.Factories;

public interface IEntityFileModelFactory
{
    EntityFileModel Create(string name, string properties, string directory);
}

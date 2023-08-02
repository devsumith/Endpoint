// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.Artifacts.Files.Services;

public interface IClassService
{
    void Create(string name, string properties, string directory);
    void UnitTestCreateFor(string name, string methods, string directory);
}



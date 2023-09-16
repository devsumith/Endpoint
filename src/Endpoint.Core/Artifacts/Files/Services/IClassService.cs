// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Threading.Tasks;

namespace Endpoint.Core.Artifacts.Files.Services;

public interface IClassService
{
    Task CreateAsync(string name, string properties, string directory);

    Task UnitTestCreateAsync(string name, string methods, string directory);
}

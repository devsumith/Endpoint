// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Threading.Tasks;

namespace Endpoint.Core.Services;

public interface IMinimalApiService
{
    Task Create(string name, string dbContextName, string entityName, string directory);
}

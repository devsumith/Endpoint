// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.Services;

public interface IMinimalApiService
{
    void Create(string name, string dbContextName, string entityName, string directory);
}


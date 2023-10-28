// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.SystemModels;

public interface ISystemContextFactory
{
    Task<ISystemContext> DddCreateAsync(string name, string microserviceName, string aggregate, string properties, string directory);
}

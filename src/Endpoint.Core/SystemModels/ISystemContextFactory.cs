// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.SystemModels;

public interface ISystemContextFactory
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="microserviceName"></param>
    /// <param name="aggregate"></param>
    /// <param name="properties"></param>
    /// <param name="directory"></param>
    /// <returns></returns>
    Task<ISystemContext> DddCreateAsync(string name, string microserviceName, string aggregate, string properties, string directory);

    /// <summary>
    /// DddSlimCreateAsync.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="aggregate"></param>
    /// <param name="properties"></param>
    /// <param name="directory"></param>
    /// <returns></returns>
    Task<ISystemContext> DddSlimCreateAsync(string name, string aggregate, string properties, string directory);
}

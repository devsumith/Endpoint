// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Extensions.DependencyInjection;

namespace Endpoint.Core.Services;

public interface IDependencyInjectionService
{
     void Add(string interfaceName, string className, string directory, ServiceLifetime? serviceLifetime = null);

    void AddHosted(string hostedServiceName, string directory);
    void AddConfigureServices(string layer, string directory);
}



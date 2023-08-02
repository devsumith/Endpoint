// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Syntax.Properties;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Endpoint.Core.Services;

public interface IDomainDrivenDesignFileService
{
    Task ServiceCreate(string name, string directory);
    Task MessageCreate(string name, List<PropertyModel> properties, string directory);
    Task MessageHandlerCreate(string name, string directory);
    Task ServiceBusMessageConsumerCreate(string name = "ServiceBusMessageConsumer", string messagesNamespace = null, string directory = null);
}


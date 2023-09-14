// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Syntax.Cqrs;
using Endpoint.Core.Syntax.Entities;
using Endpoint.Core.Syntax.Interfaces;
using Endpoint.Core.Syntax.Properties;
using System.Collections.Generic;

namespace Endpoint.Core.Syntax.Classes.Factories;

public interface IClassFactory
{
    ClassModel CreateEntity(string name, string properties);
    ClassModel CreateController(EntityModel model, string directory);
    ClassModel CreateEmptyController(string name, string directory);
    ClassModel CreateDbContext(string name, List<EntityModel> entities, string directory);
    ClassModel CreateWorker(string name, string directory);
    ClassModel CreateMessageModel();
    ClassModel CreateHubModel(string name);
    InterfaceModel CreateHubInterfaceModel(string name);
    ClassModel CreateMessageProducerWorkerModel(string name, string directory);
    Tuple<ClassModel, InterfaceModel> CreateClassAndInterface(string name);
    ClassModel CreateServiceBusMessageConsumer(string name, string messagesNamespace);
    ClassModel CreateConfigureServices(string serviceSuffix);
    Task<ClassModel> DtoExtensionsCreateAsync(ClassModel aggregate);
    Task<ClassModel> CreateRequestAsync(string requestName, string properties);
    Task<ClassModel> CreateResponseAsync(RequestType responseType, string entityName, string name = null);
}


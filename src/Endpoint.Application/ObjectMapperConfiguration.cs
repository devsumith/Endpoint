using Endpoint.Application.Commands;
using Endpoint.Core.Commands;
using Endpoint.Core.Options;
using Nelibur.ObjectMapper;
using System.Runtime.CompilerServices;

namespace Endpoint.Application;

public static class ObjectMapperConfiguration
{
    [ModuleInitializer]
    public static void  Configure()
    {
        TinyMapper.Bind<Default.Request, CreateEndpointOptions>();
        TinyMapper.Bind<CreateEndpointOptions, CreateEndpointSolutionOptions>();
        TinyMapper.Bind<AddResource.Request, AddResourceOptions>();
        TinyMapper.Bind<MicroserviceRequest, CreateCleanArchitectureMicroserviceOptions>();
        TinyMapper.Bind<MicroserviceRequest, ResolveOrCreateWorkspaceOptions>();
    }
}

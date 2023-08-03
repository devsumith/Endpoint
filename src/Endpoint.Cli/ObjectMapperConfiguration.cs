// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Options;
using Nelibur.ObjectMapper;
using System.Runtime.CompilerServices;

namespace Endpoint.Cli;

public static class ObjectMapperConfiguration
{
    [ModuleInitializer]
    public static void Configure()
    {
        TinyMapper.Bind<CreateEndpointOptions, CreateEndpointSolutionOptions>();
    }
}


﻿using Endpoint.Core.Options;

namespace Endpoint.Core.Strategies.Common
{
    public interface IEndpointGenerationStrategy
    {
        int Order { get; }
        bool CanHandle(CreateEndpointOptions request);

        void Create(CreateEndpointOptions request);
    }
}
// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Options;
using Endpoint.Core.Strategies.Common;

namespace Endpoint.Core.Generators
{
    public class EndpointGenerator
    {
        public static void Generate(CreateEndpointOptions options, IEndpointGenerationStrategyFactory factory)
        {
            factory.CreateFor(options);
        }
    }
}


// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Options;
using Endpoint.Core.Strategies;

namespace Endpoint.Core.Generators
{
    public static class AdditionalResourceGenerator
    {
        public static void Generate(AddResourceOptions options, IAdditionalResourceGenerator factory)
        {
            factory.CreateFor(options);
        }
    }
}


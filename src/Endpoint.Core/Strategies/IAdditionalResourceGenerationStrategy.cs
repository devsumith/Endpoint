// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.


using Endpoint.Core.Options;


namespace Endpoint.Core.Strategies;

public interface IAdditionalResourceGenerationStrategy
{
    int Order { get; }
    bool CanHandle(AddResourceOptions options);
    void Create(AddResourceOptions options);
}



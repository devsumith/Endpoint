// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.



namespace Endpoint.Core.Strategies.Api;

public interface IWebApplicationBuilderGenerationStrategy
{
    string Create(string @namespace, string dbContextName);
}



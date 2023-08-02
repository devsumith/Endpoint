// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.


using Endpoint.Core.Options;


namespace Endpoint.Core.Services;

public interface IInfrastructureProjectFilesGenerationStrategy
{
    void Build(SettingsModel settings);
    void BuildAdditionalResource(string additionalResource, SettingsModel settings);
}



// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.


using Endpoint.Core.Options;
using Endpoint.Core.Syntax.Entities.Legacy;


namespace Endpoint.Core.Services;

public interface IApplicationProjectFilesGenerationStrategy
{
    void Build(SettingsModel settings);
    void BuildAdditionalResource(LegacyAggregatesModel aggregatesModel, SettingsModel settings);
}



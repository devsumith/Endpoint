// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Models.Options;
using Endpoint.Core.Models.Syntax.Entities.Legacy;

namespace Endpoint.Core.Services
{
    public interface IApplicationProjectFilesGenerationStrategy
    {
        void Build(SettingsModel settings);
        void BuildAdditionalResource(LegacyAggregateModel aggregateModel, SettingsModel settings);
    }
}


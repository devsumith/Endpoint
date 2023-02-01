// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Models.Options;

namespace Endpoint.Core.Models.Artifacts.Files.Strategies
{
    public interface ILaunchSettingsFileGenerationStrategy
    {
        void Create(SettingsModel settings);
    }
}


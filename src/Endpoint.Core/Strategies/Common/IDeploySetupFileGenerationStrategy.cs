// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Options;

namespace Endpoint.Core.Strategies.Common;

public interface IDeploySetupFileGenerationStrategy
{
    void Generate(SettingsModel settings);
}


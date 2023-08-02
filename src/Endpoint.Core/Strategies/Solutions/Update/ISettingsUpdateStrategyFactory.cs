// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Options;

namespace Endpoint.Core.Strategies.Solutions.Update
{
    public interface ISettingsUpdateStrategyFactory
    {
        public void Update(SolutionSettingsModel model, string entityName, string properties);
    }
}


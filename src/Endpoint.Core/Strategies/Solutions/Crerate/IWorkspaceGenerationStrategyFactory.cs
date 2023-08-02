// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Models.Options;

namespace Endpoint.Core.Strategies.Solutions.Crerate
{
    public interface IWorkspaceGenerator
    {
        void CreateFor(WorkspaceSettingsModel model);
    }

}


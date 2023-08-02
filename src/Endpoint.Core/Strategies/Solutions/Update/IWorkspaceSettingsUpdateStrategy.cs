// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Options;

namespace Endpoint.Core.Strategies.WorkspaceSettingss.Update
{
    public interface IWorkspaceSettingsUpdateStrategy
    {
        bool CanHandle(WorkspaceSettingsModel previous, WorkspaceSettingsModel next);
        int Order { get; }
        void Update(WorkspaceSettingsModel previous, WorkspaceSettingsModel next);
    }
}


// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Models.Options;
using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core.Strategies.WorkspaceSettingss.Update;

public class WorkspaceSettingsUpdateStrategyFactory : IWorkspaceSettingsUpdateStrategyFactory
{
    private readonly IEnumerable<IWorkspaceSettingsUpdateStrategy> _workspaceSettingsUpdateStrategies;

    public WorkspaceSettingsUpdateStrategyFactory(IEnumerable<IWorkspaceSettingsUpdateStrategy> WorkspaceSettingsUpdateStrategies)
    {
        _workspaceSettingsUpdateStrategies = WorkspaceSettingsUpdateStrategies;
    }

    public void UpdateFor(WorkspaceSettingsModel previous, WorkspaceSettingsModel next)
    {
        if (previous == null)
        {
            throw new ArgumentNullException(nameof(previous));
        }

        if (next == null)
        {
            throw new ArgumentNullException(nameof(next));
        }

        var strategy = _workspaceSettingsUpdateStrategies.Where(x => x.CanHandle(previous, next)).OrderByDescending(x => x.Order).FirstOrDefault();

        if (strategy == null)
        {
            throw new InvalidOperationException("Cannot find a strategy for generation for the type ");
        }

        strategy.Update(previous, next);
    }
}


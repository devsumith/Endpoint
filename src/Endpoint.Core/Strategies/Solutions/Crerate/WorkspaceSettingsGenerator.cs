
using Endpoint.Core.Options;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Endpoint.Core.Strategies.Solutions.Crerate;

public class WorkspaceSettingsGenerator : IWorkspaceGenerator
{
    private readonly IEnumerable<IWorkspaceSettingsGenerationStrategy> _workspaceGenerationStrategies;

    public WorkspaceSettingsGenerator(IEnumerable<IWorkspaceSettingsGenerationStrategy> workspaceGenerationStrategies)
    {
        _workspaceGenerationStrategies = workspaceGenerationStrategies ?? throw new System.ArgumentNullException(nameof(workspaceGenerationStrategies));
    }

    public void CreateFor(WorkspaceSettingsModel model)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        var strategy = _workspaceGenerationStrategies.Where(x => x.CanHandle(model)).OrderByDescending(x => x.Order).FirstOrDefault();

        if (strategy == null)
        {
            throw new InvalidOperationException("Cannot find a strategy for generation for the type ");
        }

        strategy.Create(model);
    }
}



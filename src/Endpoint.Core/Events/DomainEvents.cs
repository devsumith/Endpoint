// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core.Events;

public static class DomainEvents
{
    [ThreadStatic]
    private static List<Delegate> actions;

    public static void Register<T>(Action<T> callback)
        where T : class
    {
        if (actions == null)
        {
            actions = new List<Delegate>();
        }

        var existingAction = actions.OfType<Action<T>>().SingleOrDefault();

        if (existingAction != null)
        {
            var newActions = actions;

            newActions.Remove(existingAction);

            actions = newActions;
        }

        actions.Add(callback);
    }

    public static void ClearCallbacks()
    {
        actions = null;
    }

    public static void Raise<T>(T args)
        where T : class
    {
        if (actions == null)
        {
            return;
        }

        var action = actions.OfType<Action<T>>().FirstOrDefault();

        if (action != null)
        {
            action(args);
        }
    }
}
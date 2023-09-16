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

        foreach (var action in actions.OfType<Action<T>>())
        {
            action(args);
        }
    }
}
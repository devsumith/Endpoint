// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;

namespace Enpoint.Core.Events;

public static class DomainEvents
{
    [ThreadStatic]
    private static List<Delegate> _actions;

    public static void Register<T>(Action<T> callback) where T : class
    {
        if (_actions == null)
        {
            _actions = new List<Delegate>();
        }

        _actions.Add(callback);
    }

    public static void ClearCallbacks()
    {
        _actions = null;
    }

    public static void Raise<T>(T args) where T : class
    {
        if (_actions == null) return;
        foreach (var action in _actions.OfType<Action<T>>())
        {
            action(args);
        }
    }
}
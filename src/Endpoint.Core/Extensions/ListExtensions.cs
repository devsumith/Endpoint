// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace System.Collections.Generic;

public static class ListExtensions
{
    public static bool IsLast<T>(this List<T> items, T item)
    {
        if (items.Count == 0)
        {
            return false;
        }

        T last = items[items.Count - 1];
        return item.Equals(last);
    }
}

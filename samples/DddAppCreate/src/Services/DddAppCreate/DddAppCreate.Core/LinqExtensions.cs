// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace DddAppCreate.Core;

public static class LinqExtensions
{
    public static IQueryable<T> Page<T>(this IQueryable<T> queryable,int pageIndex,int pageSize)
    {
        return queryable.Skip(pageSize * pageIndex).Take(pageSize);

    }

}


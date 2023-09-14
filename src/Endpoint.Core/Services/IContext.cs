// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.Services;

public interface IContext
{
    void Set<T>(T item)
        where T : class;

    T Get<T>()
        where T : class;
}


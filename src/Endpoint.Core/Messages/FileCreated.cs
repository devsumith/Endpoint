// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using System;

namespace Endpoint.Core.Messages;

public class FileCreated: INotification {
    public string Path { get; init; }
    public FileCreated(string path)
    {
        Path = path;
    }
}


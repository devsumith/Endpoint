// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;

namespace Endpoint.Core.Messages;

public class WorkerFileCreated: INotification {
    public WorkerFileCreated(string name, string directory)
    {
        Name = name;
        Directory = directory;
    }

    public string Name { get; set; }
    public string Directory { get; set; }
}


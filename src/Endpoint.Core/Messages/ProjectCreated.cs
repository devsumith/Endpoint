// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;

namespace Endpoint.Core.Messages;

public class ProjectCreated : INotification
{
    public string Directory { get; init; }

    public ProjectCreated(string directory)
    {
        Directory = directory;
    }

}


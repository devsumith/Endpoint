// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;

namespace Endpoint.Core.Messages;

public class ServiceFileCreated: INotification {
    public ServiceFileCreated(string interfaceName, string className, string directory)
    {
        InterfaceName = interfaceName;
        ClassName = className;
        Directory = directory;
    }

    public string InterfaceName { get; set; }
    public string ClassName { get; set; }
    public string Directory { get; set; }
}


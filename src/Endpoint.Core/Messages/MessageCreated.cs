// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;

namespace Endpoint.Core.Messages;

public class MessageCreated: INotification
{
    private readonly string _name;

    public MessageCreated(string name){
        _name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public string Name { get; set; }
}



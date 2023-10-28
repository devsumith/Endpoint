// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;

namespace DddAppCreate.Core.AggregatesModel.ToDoAggregate.Commands;

public class DeleteToDoRequest: IRequest<DeleteToDoRequest>
{
    public Guid ToDoId { get; set; }
}


public class DeleteToDoResponse { }

public class DeleteToDoHandler { }



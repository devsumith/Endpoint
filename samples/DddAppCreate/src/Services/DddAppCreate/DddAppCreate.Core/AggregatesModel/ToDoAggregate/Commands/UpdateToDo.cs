// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;

namespace DddAppCreate.Core.AggregatesModel.ToDoAggregate.Commands;

public class UpdateToDoRequest: IRequest<UpdateToDoRequest>
{
    public string Name { get; set; }
    public bool IsComplete { get; set; }
    public Guid ToDoId { get; set; }
}


public class UpdateToDoResponse { }

public class UpdateToDoHandler { }



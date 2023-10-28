// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;

namespace DddAppCreate.Core.AggregatesModel.ToDoAggregate.Commands;

public class CreateToDoRequest: IRequest<CreateToDoRequest>
{
    public string Name { get; set; }
    public bool IsComplete { get; set; }
}


public class CreateToDoResponse { }

public class CreateToDoHandler { }



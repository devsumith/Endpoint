// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;

namespace Endpoint.Core.Notifications;

public record class AggregateRequested(dynamic sender): INotification { }

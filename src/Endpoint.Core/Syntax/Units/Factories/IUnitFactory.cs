// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax.Documents;

namespace Endpoint.Core.Syntax.Units.Factories;

public interface IDocumentFactory
{
    Task<DocumentModel> CreateCommandAsync(ClassModel aggregate, RouteType routeType);
}

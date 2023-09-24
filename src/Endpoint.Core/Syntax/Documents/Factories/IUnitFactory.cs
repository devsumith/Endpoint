// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Syntax.Classes;

namespace Endpoint.Core.Syntax.Documents.Factories;

public interface IDocumentFactory
{
    Task<DocumentModel> CreateCommandAsync(ClassModel aggregate, RouteType routeType);
}

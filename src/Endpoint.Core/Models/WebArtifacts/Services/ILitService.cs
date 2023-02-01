// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Threading.Tasks;

namespace Endpoint.Core.Models.WebArtifacts.Services;

public interface ILitService
{
    Task WorkspaceCreate(string name, string rootDirectory);

}



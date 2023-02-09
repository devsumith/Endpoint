// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Models.Artifacts.Solutions;

namespace Endpoint.Core.Models.Artifacts.Folders;

public interface IFolderService
{
    FolderModel AggregateQueries(string aggregateName, string directory);
    FolderModel AggregateCommands(string aggregateName, string directory);
}
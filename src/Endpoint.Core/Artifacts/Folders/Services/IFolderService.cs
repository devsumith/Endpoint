// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Syntax.Classes;

namespace Endpoint.Core.Artifacts.Folders.Services;

public interface IFolderService
{
    FolderModel AggregateQueries(ClassModel aggregate, string directory);
    FolderModel AggregateCommands(ClassModel aggregate, string directory);
}
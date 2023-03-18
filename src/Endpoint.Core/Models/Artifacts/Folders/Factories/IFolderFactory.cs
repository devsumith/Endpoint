// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Models.Syntax.Classes;

namespace Endpoint.Core.Models.Artifacts.Folders.Factories;

public interface IFolderFactory
{
    FolderModel AggregagteCommands(ClassModel aggregate, string directory);
    FolderModel AggregagteQueries(ClassModel aggregate, string directory);
    FolderModel AngularDomainModel(string modelName, string properties, string directory);
    AggregateFolderModel Aggregate(string aggregateName, string properties, string directory);
}
// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using Endpoint.Core.Syntax.Entities;

namespace Endpoint.Core.Artifacts.Files.Factories;

public interface IFileFactory
{
    TemplatedFileModel CreateTemplate(string template, string name, string directory, string extension = ".cs", string filename = null, Dictionary<string, object> tokens = null);

    EntityFileModel Create(EntityModel model, string directory);

    CSharpTemplatedFileModel CreateCSharp(string template, string @namespace, string name, string directory, Dictionary<string, object> tokens = null);

    TemplatedFileModel LaunchSettingsJson(string projectDirectory, string projectName, int port);

    TemplatedFileModel AppSettings(string projectDirectory, string projectName, string dbContextName);

    FileModel CreateCSharp<T>(T classModel, string directory)
        where T : TypeDeclarationModel;

    FileModel CreateResponseBase(string directory);

    FileModel CreateLinqExtensions(string directory);

    FileModel CreateCoreUsings(string directory);

    FileModel CreateDbContextInterface(string directory);
}

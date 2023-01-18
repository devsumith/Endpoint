using Endpoint.Core.Models.Syntax;
using Endpoint.Core.Models.Syntax.Entities;
using System.Collections.Generic;

namespace Endpoint.Core.Models.Artifacts.Files;

public interface IFileModelFactory
{
    TemplatedFileModel CreateTemplate(string template, string name, string directory, string extension = "cs", string filename = null, Dictionary<string, object> tokens = null);
    EntityFileModel Create(EntityModel model, string directory);
    CSharpTemplatedFileModel CreateCSharp(string template, string @namespace, string name, string directory, Dictionary<string, object> tokens = null);
    TemplatedFileModel LaunchSettingsJson(string projectDirectory, string projectName, int port);        
    TemplatedFileModel AppSettings(string projectDirectory, string projectName, string dbContextName);

    FileModel CreateCSharp<T>(T classModel, string directory)
        where T : TypeDeclarationModel;

    FileModel CreateResponseBase(string directory);

    FileModel CreateIQueryableExtensions(string directory);

    FileModel CreateCoreUsings(string directory);
}

// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.IO;

namespace Endpoint.Core.Services;

public interface IFileSystem
{
    void Copy(string sourceFileName, string destFileName);
    string[] ReadAllLines(string path);
    string ReadAllText(string path);
    Stream OpenRead(string path);
    bool Exists(string path);
    bool Exists(string[] paths);
    void WriteAllLines(string path, string[] contents);
    void WriteAllText(string path, string contents);
    string ParentFolder(string path);
    void CreateDirectory(string directory);
    void Delete(string path);
    void DeleteDirectory(string directory);
    string[] GetFiles(string path, string searchPattern, SearchOption searchOption);
    string GetDirectoryName(string path);

}

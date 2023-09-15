// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Artifacts.Solutions;
using Endpoint.Core.Services;
using System.IO;

namespace Endpoint.Core.Syntax;

public class SyntaxService : ISyntaxService
{
    public SyntaxModel SyntaxModel { get; set; }
    public SolutionModel SolutionModel { get; set; }

    public SyntaxService(
        IFileProvider fileProvider,
        IFileSystem fileSystem,
        string directory)
    {
        throw new NotImplementedException();
    }
}
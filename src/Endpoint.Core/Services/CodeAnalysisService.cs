// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Artifacts.Solutions;

namespace Endpoint.Core.Services;

public class CodeAnalysisService : ICodeAnalysisService
{
    public CodeAnalysisService(
        IFileProvider fileProvider,
        IFileSystem fileSystem)
    {
    }

    public SyntaxModel SyntaxModel { get; set; }

    public SolutionModel SolutionModel { get; set; }
}
// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;
using System.IO;

namespace Endpoint.Core.Models.Artifacts.Solutions;

public class SolutionReferenceService
{
    private readonly string _solutionDirectory;
    private readonly ICommandService _commandService;
    private readonly string _name;
    public string SrcDirectory => $"{_solutionDirectory}{Path.DirectorySeparatorChar}src";
    public string Directory => $"{_solutionDirectory}";
    public SolutionReferenceService(ICommandService commandService, string solutionDirectory, string name)
    {
        _commandService = commandService;
        _solutionDirectory = solutionDirectory;
        _name = name;
    }

    public void Add(string projectFullPath)
    {
        _commandService.Start($"dotnet sln add {projectFullPath}", _solutionDirectory);
    }

    public void OpenInVisualStudio()
    {
        _commandService.Start($"start {_name}.sln", _solutionDirectory);
    }
}


// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Options;
using Endpoint.Core.Services;
using Endpoint.Core.Syntax;
using System.IO;

namespace Endpoint.Core.Strategies.Common;

public class DeploySetupFileGenerationStrategy : IDeploySetupFileGenerationStrategy
{
    private readonly IFileSystem _fileSystem;
    private readonly ITemplateLocator _templateLocator;
    private readonly ITemplateProcessor _templateProcessor;

    public DeploySetupFileGenerationStrategy(IFileSystem fileSystem, ITemplateLocator templateLocator, ITemplateProcessor templateProcessor)
    {
        _fileSystem = fileSystem;
        _templateLocator = templateLocator;
        _templateProcessor = templateProcessor;
    }
    public void Generate(SettingsModel settings)
    {
        var tokens = new TokensBuilder()
            .With("Name", (SyntaxToken)settings.SolutionName)
            .With("ApiProjectName", (SyntaxToken)$"{settings.SolutionName}.Api")
            .Build();

        var template = _templateLocator.Get("DeploySetupFile");

        var result = string.Join(Environment.NewLine, _templateProcessor.Process(template, tokens));

        _fileSystem.WriteAllText($"{settings.RootDirectory}{Path.DirectorySeparatorChar}deploy{Path.DirectorySeparatorChar}setup.ps1", result);

    }
}


// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;
using Endpoint.Core.Syntax;
using System;
using System.IO;

namespace Endpoint.Core.Strategies.Application;

public interface IIdFileGenerationStrategy
{
    void Create(string entityName, string @namepace, string directory);
}

public class IdFileGenerationStrategy : IIdFileGenerationStrategy
{
    private readonly ITemplateLocator _templateLocator;
    private readonly ITemplateProcessor _templateProcessor;
    private readonly IFileSystem _fileSystem;

    public IdFileGenerationStrategy(
        ITemplateProcessor templateProcessor,
        ITemplateLocator templateLocator,
        IFileSystem fileSystem
        )
    {
        _templateProcessor = templateProcessor ?? throw new ArgumentNullException(nameof(templateProcessor));
        _templateLocator = templateLocator ?? throw new ArgumentNullException(nameof(templateLocator));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    }

    public void Create(string entityName, string @namepace, string directory)
    {
        var tokens = new TokensBuilder()
            .With("Namespace", (SyntaxToken)@namepace)
            .With("EntityName", (SyntaxToken)entityName)
            .Build();

        var content = _templateProcessor.Process(_templateLocator.Get("IdFile"), tokens);

        _fileSystem.WriteAllText($"{directory}{Path.DirectorySeparatorChar}{((SyntaxToken)entityName).PascalCase}Id.cs", string.Join(Environment.NewLine, content));
    }
}


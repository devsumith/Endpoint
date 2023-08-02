// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.


using Endpoint.Core.Options;
using Endpoint.Core.Services;
using Endpoint.Core.Syntax;
using System.IO;


namespace Endpoint.Core.Builders;

public class QueryableExtensionsBuilder
{
    public static void Build(SettingsModel settings, ITemplateLocator templateLocator, ITemplateProcessor templateProcessor, IFileSystem fileSystem, string directory)
    {
        var template = templateLocator.Get("QueryableExtensions");

        var tokens = new TokensBuilder()
            .With(nameof(settings.RootNamespace), (SyntaxToken)settings.RootNamespace)
            .With(directory, (SyntaxToken)directory)
            .With("Namespace", (SyntaxToken)settings.ApplicationNamespace)
            .With(nameof(settings.ApplicationNamespace), (SyntaxToken)settings.ApplicationNamespace)
            .Build();

        var contents = templateProcessor.Process(template, tokens);

        fileSystem.WriteAllLines($@"{directory}{Path.DirectorySeparatorChar}QueryableExtensions.cs", contents);
    }
}



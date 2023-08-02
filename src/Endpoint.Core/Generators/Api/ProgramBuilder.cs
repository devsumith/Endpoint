// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Options;
using Endpoint.Core.Services;
using Endpoint.Core.Syntax;

namespace Endpoint.Core.Builders;

public class ProgramBuilder
{
    public static void Build(SettingsModel settings, ITemplateLocator templateLocator, ITemplateProcessor templateProcessor, IFileSystem fileSystem)
    {
        var template = templateLocator.Get("Program");

        var tokens = new TokensBuilder()
            .With(nameof(settings.InfrastructureNamespace), (SyntaxToken)settings.InfrastructureNamespace)
            .With("Directory", (SyntaxToken)settings.ApiDirectory)
            .With("Namespace", (SyntaxToken)settings.ApiNamespace)
            .With("DbContext", (SyntaxToken)settings.DbContextName)
            .Build();

        var contents = templateProcessor.Process(template, tokens);

        fileSystem.WriteAllLines($@"{settings.ApiDirectory}/Program.cs", contents);
    }
}


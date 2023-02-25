// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Messages;
using Endpoint.Core.Models.Syntax;
using Endpoint.Core.Services;
using MediatR;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Core.MessageHandlers;

public class AddArrangeActAssertFileCreatedHandler : INotificationHandler<FileCreated>
{
    private readonly IArtifactGenerationStrategyFactory _artifactGenerationStrategyFactory;
    private readonly ISyntaxGenerationStrategyFactory _syntaxGenerationStrategyFactory;
    private readonly IFileSystem _fileSystem;
    public AddArrangeActAssertFileCreatedHandler(
        IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory,
        ISyntaxGenerationStrategyFactory syntaxGenerationStrategyFactory,
        IFileSystem fileSystem)
    {
        _artifactGenerationStrategyFactory = artifactGenerationStrategyFactory;
        _fileSystem = fileSystem;
        _syntaxGenerationStrategyFactory = syntaxGenerationStrategyFactory;
    }

    public async Task Handle(FileCreated notification, CancellationToken cancellationToken)
    {
        //
        //TODO: fix this later

        return;

        if (!notification.Path.EndsWith("spec.ts"))
            return;

        var content = _fileSystem.ReadAllText(notification.Path);

        if (content.Contains("// ARRANGE") && content.Contains("// ACT") && content.Contains("// ASSERT"))
            return;

        var contents = content.Split(Environment.NewLine);

        var contentBuilder = new StringBuilder();

        var sut = ((SyntaxToken)Path.GetFileNameWithoutExtension(notification.Path).Split('.').First()).PascalCase();

        foreach (var line in contents)
        {
            if (line.Contains("should create"))
            {
                var testRef = _syntaxGenerationStrategyFactory.CreateFor(new TestReferenceModel()
                {
                    SystemUnderTestName = sut
                });

                contentBuilder.AppendLine(testRef);
            }

            contentBuilder.AppendLine(line);

            if (line.Contains("beforeEach"))
            {
                contentBuilder.AppendLine("// ARRANGE".Indent(2, 2));
            }

            if (line.Contains(" fixture.componentInstance"))
            {
                contentBuilder.AppendLine("// ACT".Indent(2, 2));
            }

            if (line.Contains("should create"))
            {
                contentBuilder.AppendLine("// ASSERT".Indent(2, 2));
            }
        }

        _fileSystem.WriteAllText(notification.Path, contentBuilder.ToString());
    }
}
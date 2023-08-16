// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Artifacts;
using Endpoint.Core.Messages;
using Endpoint.Core.Services;
using MediatR;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Endpoint.Core.MessageHandlers;

public class AddArrangeActAssertFileCreatedHandler : INotificationHandler<FileCreated>
{
    private readonly IArtifactGenerator _artifactGenerator;
    private readonly ISyntaxGenerator _syntaxGenerator;
    private readonly IFileSystem _fileSystem;
    public AddArrangeActAssertFileCreatedHandler(
        IArtifactGenerator artifactGenerator,
        ISyntaxGenerator syntaxGenerator,
        IFileSystem fileSystem)
    {
        _artifactGenerator = artifactGenerator;
        _fileSystem = fileSystem;
        _syntaxGenerator = syntaxGenerator;
    }

    public async Task Handle(FileCreated notification, CancellationToken cancellationToken)
    {
        //
        //TODO: fix this later

        return;

        if (!notification.Path.EndsWith("spec.ts"))
            return;

        var content = _fileSystem.File.ReadAllText(notification.Path);

        if (content.Contains("// ARRANGE") && content.Contains("// ACT") && content.Contains("// ASSERT"))
            return;

        var contents = content.Split(Environment.NewLine);

        var contentBuilder = new StringBuilder();

        var sut = ((SyntaxToken)Path.GetFileNameWithoutExtension(notification.Path).Split('.').First()).PascalCase();

        foreach (var line in contents)
        {
            if (line.Contains("should create"))
            {
                var testRef = await _syntaxGenerator.GenerateAsync(new TestReferenceModel()
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

        _fileSystem.File.WriteAllText(notification.Path, contentBuilder.ToString());
    }
}
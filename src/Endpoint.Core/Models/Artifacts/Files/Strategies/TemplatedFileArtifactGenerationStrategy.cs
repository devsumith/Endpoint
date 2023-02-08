// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Internals;
using Endpoint.Core.Messages;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;
using System.Reactive.Linq;

namespace Endpoint.Core.Models.Artifacts.Files.Strategies;

public class TemplatedFileArtifactGenerationStrategy : ArtifactGenerationStrategyBase<TemplatedFileModel>
{
    private readonly ILogger<TemplatedFileArtifactGenerationStrategy> _logger;
    private readonly ITemplateLocator _templateLocator;
    private readonly IFileSystem _fileSystem;
    private readonly ITemplateProcessor _templateProcessor;
    private readonly ISolutionNamespaceProvider _solutionNamespaceProvider;
    private readonly Observable<INotification> _observableNotifications;

    public TemplatedFileArtifactGenerationStrategy(
        IServiceProvider serviceProvider,
        ITemplateProcessor templateProcessor,
        ITemplateLocator templateLocator,
        IFileSystem fileSystem,
        ISolutionNamespaceProvider solutionNamespaceProvider,
        ILogger<TemplatedFileArtifactGenerationStrategy> logger,
        Observable<INotification> observableNotifications)
        : base(serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _templateProcessor = templateProcessor ?? throw new ArgumentNullException(nameof(templateProcessor));
        _templateLocator = templateLocator ?? throw new ArgumentNullException(nameof(templateLocator));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _solutionNamespaceProvider = solutionNamespaceProvider ?? throw new ArgumentNullException(nameof(solutionNamespaceProvider));
        _observableNotifications = observableNotifications ?? throw new ArgumentNullException(nameof(observableNotifications));
    }

    public override void Create(IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory, TemplatedFileModel model, dynamic context = null)
    {
        _logger.LogInformation("Generating artifact for {0}.", model);

        var template = _templateLocator.Get(model.Template);

        var tokens = new TokensBuilder()
            .With("SolutionNamespace", _solutionNamespaceProvider.Get(model.Directory))
            .Build();

        foreach (var token in tokens)
        {
            model.Tokens.Add(token.Key, token.Value);
        }

        var result = _templateProcessor.Process(template, model.Tokens);

        var parts = Path.GetDirectoryName(model.Path).Split(System.IO.Path.DirectorySeparatorChar);

        for(var i = 1; i <= parts.Length; i++)
        {
            var dir = string.Join(Path.DirectorySeparatorChar, parts.Take(i));

            if (!Directory.Exists(dir))
                _fileSystem.CreateDirectory(dir);
        }

        _fileSystem.WriteAllText(model.Path, string.Join(Environment.NewLine, result));

        _observableNotifications.Broadcast(new FileCreated(model.Path));

    }
}

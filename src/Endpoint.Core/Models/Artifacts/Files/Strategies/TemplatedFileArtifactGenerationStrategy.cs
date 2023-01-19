using Endpoint.Core.Abstractions;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Models.Artifacts.Files.Strategies;

public class TemplatedFileArtifactGenerationStrategy : ArtifactGenerationStrategyBase<TemplatedFileModel>
{
    private readonly ILogger<TemplatedFileArtifactGenerationStrategy> _logger;
    private readonly ITemplateLocator _templateLocator;
    private readonly IFileSystem _fileSystem;
    private readonly ITemplateProcessor _templateProcessor;
    private readonly ISolutionNamespaceProvider _solutionNamespaceProvider;

    public TemplatedFileArtifactGenerationStrategy(
        IServiceProvider serviceProvider,
        ITemplateProcessor templateProcessor,
        ITemplateLocator templateLocator,
        IFileSystem fileSystem,
        ISolutionNamespaceProvider solutionNamespaceProvider,
        ILogger<TemplatedFileArtifactGenerationStrategy> logger)
        : base(serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _templateProcessor = templateProcessor ?? throw new ArgumentNullException(nameof(templateProcessor));
        _templateLocator = templateLocator ?? throw new ArgumentNullException(nameof(templateLocator));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _solutionNamespaceProvider = solutionNamespaceProvider ?? throw new ArgumentNullException(nameof(solutionNamespaceProvider));
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

        var result = _templateProcessor.Process(template, model.Tokens); ;

        _fileSystem.WriteAllText(model.Path, string.Join(Environment.NewLine, result));

    }
}
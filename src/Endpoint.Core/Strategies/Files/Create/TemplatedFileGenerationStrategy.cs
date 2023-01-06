using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Artifacts;
using Endpoint.Core.Services;
using Endpoint.Core.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Strategies.Files.Create;

public class TemplatedFileGenerationStrategy : ArtifactGenerationStrategyStrategyBase<TemplatedFileModel>
{
    private readonly IFileSystem _fileSystem;
    private readonly ITemplateLocator _templateLocator;
    private readonly ITemplateProcessor _templateProcessor;
    private readonly ISolutionNamespaceProvider _solutionNamespaceProvider;
    private readonly ILogger<TemplatedFileGenerationStrategy> _logger;

    public TemplatedFileGenerationStrategy(
        IFileSystem fileSystem,
        ITemplateLocator templateLocator,
        ITemplateProcessor templateProcessor,
        ISolutionNamespaceProvider solutionNamespaceProvider,
        IServiceProvider serviceProvider,
        ILogger<TemplatedFileGenerationStrategy> logger
        ): base(serviceProvider)
    {
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _templateProcessor = templateProcessor ?? throw new ArgumentNullException(nameof(templateProcessor));
        _templateLocator = templateLocator ?? throw new ArgumentNullException(nameof(templateLocator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _solutionNamespaceProvider = solutionNamespaceProvider ?? throw new ArgumentNullException(nameof(solutionNamespaceProvider));
    }

    public int Order => 0;

    public override void Create(IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory, TemplatedFileModel model, dynamic configuration = null)
    {
        _logger.LogInformation($"Creating {model.Name} file at {model.Path}");

        var template = _templateLocator.Get(model.Template);

        var tokens = new TokensBuilder()
            .With("SolutionNamespace", (Token)_solutionNamespaceProvider.Get(model.Directory))
            .Build();

        foreach (var token in tokens)
        {
            model.Tokens.Add(token.Key, token.Value);
        }

        var result = _templateProcessor.Process(template, model.Tokens); ;

        _fileSystem.WriteAllText(model.Path, string.Join(Environment.NewLine, result));
    }
}

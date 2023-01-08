using Endpoint.Core.Abstractions;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text;

namespace Endpoint.Core.Models.Artifacts.Files;


public class ObjectFileArtifactGenerationStrategyBase<T> : ArtifactGenerationStrategyBase<ObjectFileModel<T>>
    where T : class
{
    private readonly ILogger<ObjectFileArtifactGenerationStrategyBase<T>> _logger;
    private readonly ISyntaxGenerationStrategyFactory _syntaxGenerationStrategyFactory;
    private readonly IFileSystem _fileSystem;

    public ObjectFileArtifactGenerationStrategyBase(
        IServiceProvider serviceProvider,
        ISyntaxGenerationStrategyFactory syntaxGenerationStrategyFactory,
        IFileSystem fileSystem,
        ILogger<ObjectFileArtifactGenerationStrategyBase<T>> logger)
        : base(serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _syntaxGenerationStrategyFactory = syntaxGenerationStrategyFactory ?? throw new ArgumentNullException(nameof(syntaxGenerationStrategyFactory));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    }

    public override void Create(IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory, ObjectFileModel<T> model, dynamic configuration = null)
    {
        _logger.LogInformation("Generating artifact for {0}.", model);

        var builder = new StringBuilder();

        foreach (var @using in model.Usings)
        {
            builder.AppendLine($"using {@using.Name};");

            if (@using == model.Usings.Last())
            {
                builder.AppendLine();
            }
        }

        builder.AppendLine(_syntaxGenerationStrategyFactory.CreateFor(model.Object));

        _fileSystem.WriteAllText(model.Path, builder.ToString());
    }
}
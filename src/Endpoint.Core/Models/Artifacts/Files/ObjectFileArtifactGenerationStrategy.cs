using Endpoint.Core.Abstractions;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text;

namespace Endpoint.Core.Models.Artifacts.Files;

public class ContentFileArtifactGenerationStrategy : ArtifactGenerationStrategyBase<ContentFileModel>
{
    private readonly IFileSystem _fileSystem;
    public ContentFileArtifactGenerationStrategy(
        IServiceProvider serviceProvider,
        IFileSystem fileSystem
        ) 
        : base(serviceProvider)
    {
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    }

    public override void Create(IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory, ContentFileModel model, dynamic context = null)
    {
        _fileSystem.WriteAllText(model.Path, model.Content);
    }
}

public class DbContextFileFromCoreDirectoryArtifactGenerationStrategy : ArtifactGenerationStrategyBase<string>
{
    private readonly IFileProvider _fileProvider;

    public DbContextFileFromCoreDirectoryArtifactGenerationStrategy(IServiceProvider serviceProvider, IFileProvider fileProvider) : base(serviceProvider)
    {
        _fileProvider = fileProvider;
    }

    public override bool CanHandle(object model, dynamic context = null)
    {
        if(model is string value)
        {
            var projectDirectory = _fileProvider.Get("*.csproj", value);
        }

        return false;
    }

    public override void Create(IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory, string directory, dynamic context = null)
    {
        throw new NotImplementedException();
    }
}

public class ObjectFileArtifactGenerationStrategyBase<T> : ArtifactGenerationStrategyBase<ObjectFileModel<T>>
    where T : class
{
    private readonly ILogger<ObjectFileArtifactGenerationStrategyBase<T>> _logger;
    private readonly ISyntaxGenerationStrategyFactory _syntaxGenerationStrategyFactory;
    private readonly IFileSystem _fileSystem;
    private readonly INamespaceProvider _namespaceProvider;

    public ObjectFileArtifactGenerationStrategyBase(
        IServiceProvider serviceProvider,
        ISyntaxGenerationStrategyFactory syntaxGenerationStrategyFactory,
        IFileSystem fileSystem,
        INamespaceProvider namespaceProvider,
        ILogger<ObjectFileArtifactGenerationStrategyBase<T>> logger)
        : base(serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _syntaxGenerationStrategyFactory = syntaxGenerationStrategyFactory ?? throw new ArgumentNullException(nameof(syntaxGenerationStrategyFactory));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _namespaceProvider = namespaceProvider ?? throw new ArgumentNullException(nameof(namespaceProvider));
    }

    public override void Create(IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory, ObjectFileModel<T> model, dynamic context = null)
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

        var fileNamespace = string.IsNullOrEmpty(model.Namespace) ? _namespaceProvider.Get(model.Directory) : model.Namespace;

        builder.AppendLine($"namespace {fileNamespace};");

        builder.AppendLine();

        builder.AppendLine(_syntaxGenerationStrategyFactory.CreateFor(model.Object, context));

        _fileSystem.WriteAllText(model.Path, builder.ToString());
    }
}
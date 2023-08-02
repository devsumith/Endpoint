
using Endpoint.Core.Options;
using Endpoint.Core.Services;
using System.IO;


namespace Endpoint.Core.Strategies.Common;

public interface IGitIgnoreFileGenerationStrategy
{
    void Generate(SettingsModel settings);
}

public class GitIgnoreFileGenerationStrategy : IGitIgnoreFileGenerationStrategy
{
    private readonly IFileSystem _fileSystem;
    private readonly ITemplateLocator _templateLocator;

    public GitIgnoreFileGenerationStrategy(IFileSystem fileSystem, ITemplateLocator templateLocator)
    {
        _fileSystem = fileSystem;
        _templateLocator = templateLocator;
    }
    public void Generate(SettingsModel settings)
    {
        _fileSystem.WriteAllText($"{settings.RootDirectory}{Path.DirectorySeparatorChar}.gitignore", string.Join(Environment.NewLine, _templateLocator.Get("GitIgnoreFile")));

    }
}


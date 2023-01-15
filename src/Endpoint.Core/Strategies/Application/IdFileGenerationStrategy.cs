using Endpoint.Core.Services;
using Endpoint.Core.ValueObjects;
using System;
using System.IO;

namespace Endpoint.Core.Strategies.Application;

public interface IIdFileGenerationStrategy
{
    void Create(string entityName, string @namepace, string directory);
}

public class IdFileGenerationStrategy : IIdFileGenerationStrategy
{
    private readonly ITemplateLocator _templateLocator;
    private readonly ITemplateProcessor _templateProcessor;
    private readonly IFileSystem _fileSystem;

    public IdFileGenerationStrategy(
        ITemplateProcessor templateProcessor,
        ITemplateLocator templateLocator,
        IFileSystem fileSystem
        )
    {
        _templateProcessor = templateProcessor ?? throw new ArgumentNullException(nameof(templateProcessor));
        _templateLocator = templateLocator ?? throw new ArgumentNullException(nameof(templateLocator));    
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    }

    public void Create(string entityName, string @namepace, string directory)
    {
        var tokens = new TokensBuilder()
            .With("Namespace",(Token)@namepace)
            .With("EntityName", (Token)entityName)
            .Build();

        var content = _templateProcessor.Process(_templateLocator.Get("IdFile"), tokens);

        _fileSystem.WriteAllText($"{directory}{Path.DirectorySeparatorChar}{((Token)entityName).PascalCase}Id.cs", string.Join(Environment.NewLine, content));
    }
}

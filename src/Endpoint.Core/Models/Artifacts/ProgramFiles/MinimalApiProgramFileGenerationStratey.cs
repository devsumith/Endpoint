using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Syntax;
using Endpoint.Core.Models.Syntax.Classes;
using Endpoint.Core.Services;
using Endpoint.Core.Strategies.Api;
using Endpoint.Core.Strategies.Infrastructure;
using System.Collections.Generic;
using System.IO;

namespace Endpoint.Core.Models.Artifacts.ProgramFiles;

public class MinimalApiProgramFileGenerationStratey : IMinimalApiProgramFileGenerationStratey
{
    private readonly IFileSystem _fileSystem;
    private readonly IWebApplicationBuilderGenerationStrategy _webApplicationBuilderGenerationStrategy;
    private readonly IWebApplicationGenerationStrategy _webApplicationGenerationStrategy;
    private readonly ISyntaxGenerationStrategyFactory _codeBlockGenerationStrategyFactory;
    public MinimalApiProgramFileGenerationStratey(
        IFileSystem fileSystem,
        IWebApplicationGenerationStrategy webApplicationGenerationStrategy,
        IWebApplicationBuilderGenerationStrategy webApplicationBuilderGenerationStrategy,
        ISyntaxGenerationStrategyFactory codeBlockGenerationStrategyFactory
        )
    {
        _fileSystem = fileSystem;
        _webApplicationBuilderGenerationStrategy = webApplicationBuilderGenerationStrategy;
        _webApplicationGenerationStrategy = webApplicationGenerationStrategy;
        _codeBlockGenerationStrategyFactory = codeBlockGenerationStrategyFactory;
    }

    public void Create(MinimalApiProgramModel model, string directory)
    {
        var content = new List<string>();

        foreach (var @using in model.Usings)
        {
            content.Add($"using {@using};");
        }

        content.Add("");

        content.Add(_webApplicationBuilderGenerationStrategy.Create(default, default));

        content.Add("");

        content.Add(_webApplicationGenerationStrategy.Create(default, default, default));

        content.Add("");

        foreach (var entity in model.Entities)
        {
            content.Add(_codeBlockGenerationStrategyFactory.CreateFor(entity));

            content.Add("");
        }

        content.Add(new DbContextGenerationStrategy().Create(new DbContextModel(model.DbContextName, model.Entities)));

        _fileSystem.WriteAllText($"{directory}{Path.DirectorySeparatorChar}Program.cs", string.Join(Environment.NewLine, content));
    }
}

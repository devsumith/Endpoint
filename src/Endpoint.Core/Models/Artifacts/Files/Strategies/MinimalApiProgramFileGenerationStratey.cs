// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Services;
using Endpoint.Core.Strategies.Api;
using System.Text;

namespace Endpoint.Core.Models.Artifacts.Files.Strategies;

public class MinimalApiProgramFileGenerationStratey : ArtifactGenerationStrategyBase<MinimalApiProgramFileModel>
{
    private readonly IFileSystem _fileSystem;
    private readonly IWebApplicationBuilderGenerationStrategy _webApplicationBuilderGenerationStrategy;
    private readonly IWebApplicationGenerationStrategy _webApplicationGenerationStrategy;
    private readonly ISyntaxGenerationStrategyFactory _syntaxGenerationStrategyFactory;

    public MinimalApiProgramFileGenerationStratey(
        IServiceProvider serviceProvider,
        IFileSystem fileSystem,
        IWebApplicationGenerationStrategy webApplicationGenerationStrategy,
        IWebApplicationBuilderGenerationStrategy webApplicationBuilderGenerationStrategy,
        ISyntaxGenerationStrategyFactory symtaxGenerationStrategyFactory
        ) : base(serviceProvider)
    {
        _fileSystem = fileSystem;
        _webApplicationBuilderGenerationStrategy = webApplicationBuilderGenerationStrategy;
        _webApplicationGenerationStrategy = webApplicationGenerationStrategy;
        _syntaxGenerationStrategyFactory = symtaxGenerationStrategyFactory;
    }

    public override void Create(IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory, MinimalApiProgramFileModel model, dynamic context = null)
    {

        var builder = new StringBuilder();

        foreach (var @using in model.Usings)
        {
            builder.AppendLine($"using {@using};");
        }

        builder.AppendLine();

        builder.AppendLine(_syntaxGenerationStrategyFactory.CreateFor(model.WebApplicationBuilder));

        builder.AppendLine();

        builder.AppendLine(_syntaxGenerationStrategyFactory.CreateFor(model.WebApplication));

        _fileSystem.WriteAllText(model.Path, builder.ToString());
    }
}


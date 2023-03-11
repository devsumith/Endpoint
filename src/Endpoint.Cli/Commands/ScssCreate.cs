// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Artifacts.Files;
using Endpoint.Core.Models.Artifacts.Files.Factories;
using Endpoint.Core.Models.WebArtifacts.Services;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Cli.Commands;


[Verb("scss-create")]
public class ScssCreateRequest : IRequest {
    [Option('n',"name")]
    public string Name { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class ScssCreateRequestHandler : IRequestHandler<ScssCreateRequest>
{
    private readonly ILogger<ScssCreateRequestHandler> _logger;
    private readonly IFileModelFactory _fileModelFactory;
    private readonly IArtifactGenerationStrategyFactory _artifactGenerationStrategyFactory;
    private readonly INamingConventionConverter _namingConventionConverter;
    private readonly IFileProvider _fileProvider;
    private readonly IAngularService _angularService;
    public ScssCreateRequestHandler(
        ILogger<ScssCreateRequestHandler> logger,
        IFileModelFactory fileModelFactory,
        IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory,
        INamingConventionConverter namingConventionConverter,
        IFileProvider fileProvider,
        IAngularService angularService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fileModelFactory = fileModelFactory ?? throw new ArgumentNullException(nameof(fileModelFactory));
        _artifactGenerationStrategyFactory = artifactGenerationStrategyFactory ?? throw new ArgumentNullException(nameof(artifactGenerationStrategyFactory));
        _namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter));
        _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        _angularService = angularService ?? throw new ArgumentNullException(nameof(angularService));
    }

    public async Task Handle(ScssCreateRequest request, CancellationToken cancellationToken)
    {
        var tokens = new TokensBuilder().With("prefix", "g").Build();

        FileModel model = null!;

        var scssTemplates = new string[] {
            "Actions",
            "Brand",
            "Breakpoints",
            "Buttons",
            "Dialogs",
            "Field",
            "FormFields",
            "Header",
            "HeadingGroup",
            "PanelHeading",
            "Label",
            "Pills",
            "RouterLinkActive",
            "Table",
            "Textarea",
            "Title",
            "TitleBar",
            "Variables"
        };

        if(string.IsNullOrEmpty(request.Name))
        {
            foreach(var name in scssTemplates)
            {
                model = _fileModelFactory.CreateTemplate(name, $"_{_namingConventionConverter.Convert(NamingConvention.SnakeCase, name)}", request.Directory, "scss", tokens: tokens);

                _artifactGenerationStrategyFactory.CreateFor(model);
            }
        }
        else
        {
            var nameSnakeCase = _namingConventionConverter.Convert(NamingConvention.SnakeCase, request.Name);

            var scssTemplate = scssTemplates.FirstOrDefault(x => x.ToLower() == request.Name.ToLower());

            model = scssTemplate == null
                ? new ContentFileModel(string.Empty, $"_{nameSnakeCase}", request.Directory, "scss")
                : _fileModelFactory.CreateTemplate(request.Name, $"_{nameSnakeCase}", request.Directory, "scss", tokens: tokens);

            _artifactGenerationStrategyFactory.CreateFor(model);
        }

        _angularService.IndexCreate(true, request.Directory);

    }
}

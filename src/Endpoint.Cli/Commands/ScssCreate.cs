// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts;
using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Artifacts.Files.Factories;
using Endpoint.Core.Artifacts.Services;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Cli.Commands;

[Verb("scss-create")]
public class ScssCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class ScssCreateRequestHandler : IRequestHandler<ScssCreateRequest>
{
    private readonly ILogger<ScssCreateRequestHandler> logger;
    private readonly IFileFactory fileFactory;
    private readonly IArtifactGenerator artifactGenerator;
    private readonly INamingConventionConverter namingConventionConverter;
    private readonly IFileProvider fileProvider;
    private readonly IAngularService angularService;

    public ScssCreateRequestHandler(
        ILogger<ScssCreateRequestHandler> logger,
        IFileFactory fileFactory,
        IArtifactGenerator artifactGenerator,
        INamingConventionConverter namingConventionConverter,
        IFileProvider fileProvider,
        IAngularService angularService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.fileFactory = fileFactory ?? throw new ArgumentNullException(nameof(fileFactory));
        this.artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
        this.namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter));
        this.fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        this.angularService = angularService ?? throw new ArgumentNullException(nameof(angularService));
    }

    public async Task Handle(ScssCreateRequest request, CancellationToken cancellationToken)
    {
        var tokens = new TokensBuilder().With("prefix", "g").Build();

        FileModel model = null!;

        var scssTemplates = new string[]
        {
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
            "Variables",
        };

        if (string.IsNullOrEmpty(request.Name))
        {
            foreach (var name in scssTemplates)
            {
                model = fileFactory.CreateTemplate(name, $"_{namingConventionConverter.Convert(NamingConvention.SnakeCase, name)}", request.Directory, ".scss", tokens: tokens);

                await artifactGenerator.GenerateAsync(model);
            }
        }
        else
        {
            var nameSnakeCase = namingConventionConverter.Convert(NamingConvention.SnakeCase, request.Name);

            var scssTemplate = scssTemplates.FirstOrDefault(x => x.ToLower() == request.Name.ToLower());

            model = scssTemplate == null
                ? new ContentFileModel(string.Empty, $"_{nameSnakeCase}", request.Directory, ".scss")
                : fileFactory.CreateTemplate(request.Name, $"_{nameSnakeCase}", request.Directory, ".scss", tokens: tokens);

            await artifactGenerator.GenerateAsync(model);
        }

        await angularService.IndexCreate(true, request.Directory);
    }
}

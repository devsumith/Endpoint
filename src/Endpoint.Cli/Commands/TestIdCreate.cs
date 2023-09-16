// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Cli.Commands;

[Verb("test-header-create")]
public class TestHeaderCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class TestIdCreateRequestHandler : IRequestHandler<TestHeaderCreateRequest>
{
    private readonly ILogger<TestIdCreateRequestHandler> logger;
    private readonly IClipboardService clipboardService;
    private readonly ITemplateLocator templateLocator;
    private readonly ITemplateProcessor templateProcessor;

    public TestIdCreateRequestHandler(
        ILogger<TestIdCreateRequestHandler> logger,
        IClipboardService clipboardService,
        ITemplateLocator templateLocator,
        ITemplateProcessor templateProcessor)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.clipboardService = clipboardService ?? throw new ArgumentNullException(nameof(clipboardService));
        this.templateLocator = templateLocator ?? throw new ArgumentNullException(nameof(templateLocator));
        this.templateProcessor = templateProcessor ?? throw new ArgumentNullException(nameof(templateProcessor));
    }

    public async Task Handle(TestHeaderCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(TestIdCreateRequestHandler));

        var template = string.Join(Environment.NewLine, templateLocator.Get("TestHeader"));

        var result = templateProcessor.Process(template, new TokensBuilder()
            .With("SystemUnderTest", request.Name)
            .Build());

        clipboardService.SetText(result);
    }
}

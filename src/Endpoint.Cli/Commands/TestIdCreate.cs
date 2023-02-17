// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Endpoint.Core.Services;

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
    private readonly ILogger<TestIdCreateRequestHandler> _logger;
    private readonly IClipboardService _clipboardService;
    private readonly ITemplateLocator _templateLocator;
    private readonly ITemplateProcessor _templateProcessor;

    public TestIdCreateRequestHandler(
        ILogger<TestIdCreateRequestHandler> logger,
        IClipboardService clipboardService,
        ITemplateLocator templateLocator,
        ITemplateProcessor templateProcessor)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _clipboardService = clipboardService ?? throw new ArgumentNullException(nameof(clipboardService));
        _templateLocator = templateLocator ?? throw new ArgumentNullException(nameof(templateLocator));
        _templateProcessor = templateProcessor ?? throw new ArgumentNullException(nameof(templateProcessor));
    }

    public async Task Handle(TestHeaderCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(TestIdCreateRequestHandler));

        var template = string.Join(Environment.NewLine, _templateLocator.Get("TestHeader"));

        var result = _templateProcessor.Process(template, new TokensBuilder()
            .With("SystemUnderTest", request.Name)
            .Build());

        _clipboardService.SetText(result);


    }
}

// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts.AngularProjects;
using Endpoint.Core.Artifacts.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Cli.Commands;

[Verb("update-compiler-options-to-use-jest-types")]
public class UpdateCompilerOptionsToUseJestTypesRequest : IRequest
{
    [Option('n', "name")]
    public string ProjectName { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class UpdateCompilerOptionsToUseJestTypesRequestHandler : IRequestHandler<UpdateCompilerOptionsToUseJestTypesRequest>
{
    private readonly ILogger<UpdateCompilerOptionsToUseJestTypesRequestHandler> logger;
    private readonly IAngularService angularService;

    public UpdateCompilerOptionsToUseJestTypesRequestHandler(
        ILogger<UpdateCompilerOptionsToUseJestTypesRequestHandler> logger,
        IAngularService angularService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.angularService = angularService ?? throw new ArgumentNullException(nameof(angularService));
    }

    public async Task Handle(UpdateCompilerOptionsToUseJestTypesRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(UpdateCompilerOptionsToUseJestTypesRequestHandler));

        var angularProjectModel = new AngularProjectModel(request.ProjectName, request.Directory, null, request.Directory);

        await angularService.UpdateCompilerOptionsToUseJestTypes(angularProjectModel);
    }
}

// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Endpoint.Core.Models.WebArtifacts.Services;
using Endpoint.Core.Models.WebArtifacts;

namespace Endpoint.Cli.Commands;


[Verb("update-compiler-options-to-use-jest-types")]
public class UpdateCompilerOptionsToUseJestTypesRequest : IRequest {
    [Option('n',"name")]
    public string ProjectName { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class UpdateCompilerOptionsToUseJestTypesRequestHandler : IRequestHandler<UpdateCompilerOptionsToUseJestTypesRequest>
{
    private readonly ILogger<UpdateCompilerOptionsToUseJestTypesRequestHandler> _logger;
    private readonly IAngularService _angularService;

    public UpdateCompilerOptionsToUseJestTypesRequestHandler(
        ILogger<UpdateCompilerOptionsToUseJestTypesRequestHandler> logger,
        IAngularService angularService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _angularService = angularService ?? throw new ArgumentNullException(nameof(angularService));
    }

    public async Task Handle(UpdateCompilerOptionsToUseJestTypesRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(UpdateCompilerOptionsToUseJestTypesRequestHandler));

        var angularProjectModel = new AngularProjectModel(request.ProjectName, request.Directory, null,request.Directory);

        _angularService.UpdateCompilerOptionsToUseJestTypes(angularProjectModel);


    }
}

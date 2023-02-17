// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using Endpoint.Core.Services;
using System.Collections.Generic;
using Endpoint.Core.Models.Syntax.Properties;
using Endpoint.Core.Enums;
using Endpoint.Core.Models.Syntax.Types;
using Endpoint.Core.Models.Syntax;

namespace Endpoint.Cli.Commands;


[Verb("message-create")]
public class MessageCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }

    [Option('p', "properties")]
    public string Properties { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class MessageCreateRequestHandler : IRequestHandler<MessageCreateRequest>
{
    private readonly ILogger<MessageCreateRequestHandler> _logger;
    private readonly IDomainDrivenDesignFileService _domainDrivenDesignFileService;

    public MessageCreateRequestHandler(
        IDomainDrivenDesignFileService domainDrivenDesignFileService,
        ILogger<MessageCreateRequestHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _domainDrivenDesignFileService = domainDrivenDesignFileService ?? throw new ArgumentNullException(nameof(domainDrivenDesignFileService));
    }

    public async Task Handle(MessageCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(MessageCreateRequestHandler));

        var properties = new List<PropertyModel>();

        if (!string.IsNullOrEmpty(request.Properties))
            foreach (var prop in request.Properties.Split(','))
            {
                var parts = prop.Split(':');
                var name = parts[0];
                var type = parts[1];

                properties.Add(new PropertyModel(default, AccessModifier.Public, new TypeModel(type), name, PropertyAccessorModel.GetSet));

            }

        _domainDrivenDesignFileService.MessageCreate(request.Name, properties, request.Directory);
    }
}

// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts.Units;
using Endpoint.Core.Syntax;
using Endpoint.Core.Syntax.Properties;
using Endpoint.Core.Syntax.Types;
using MediatR;
using Microsoft.Extensions.Logging;

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
    private readonly ILogger<MessageCreateRequestHandler> logger;
    private readonly IDomainDrivenDesignFileService domainDrivenDesignFileService;

    public MessageCreateRequestHandler(
        IDomainDrivenDesignFileService domainDrivenDesignFileService,
        ILogger<MessageCreateRequestHandler> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.domainDrivenDesignFileService = domainDrivenDesignFileService ?? throw new ArgumentNullException(nameof(domainDrivenDesignFileService));
    }

    public async Task Handle(MessageCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(MessageCreateRequestHandler));

        var properties = new List<PropertyModel>();

        if (!string.IsNullOrEmpty(request.Properties))
        {
            foreach (var prop in request.Properties.Split(','))
            {
                var parts = prop.Split(':');
                var name = parts[0];
                var type = parts[1];

                properties.Add(new PropertyModel(default, AccessModifier.Public, new TypeModel(type), name, PropertyAccessorModel.GetSet));
            }
        }

        domainDrivenDesignFileService.MessageCreate(request.Name, properties, request.Directory);
    }
}

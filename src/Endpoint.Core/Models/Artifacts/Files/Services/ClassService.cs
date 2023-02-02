// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Enums;
using Endpoint.Core.Models.Syntax.Classes;
using Endpoint.Core.Models.Syntax.Properties;
using Endpoint.Core.Models.Syntax.Types;
using Endpoint.Core.Models.Syntax;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Endpoint.Core.Models.Artifacts.Files.Services;

public class ClassService: IClassService
{
    private readonly ILogger<ClassService> _logger;
    private readonly IArtifactGenerationStrategyFactory _artifactGenerationStrategyFactory;

    public ClassService(ILogger<ClassService> logger, IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory)
    {
        _artifactGenerationStrategyFactory = artifactGenerationStrategyFactory;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void Create(string name, string properties, string directory)
    {
        _logger.LogInformation("Create Class {name}", name);

        var @class = new ClassModel(name);

        @class.UsingDirectives.Add(new UsingDirectiveModel() { Name = "System" });

        if (!string.IsNullOrEmpty(properties))
            foreach (var property in properties.Split(','))
            {
                var parts = property.Split(':');
                var propertyName = parts[0];
                var propertyType = parts[1];

                @class.Properties.Add(new PropertyModel(@class, AccessModifier.Public, new TypeModel() { Name = propertyType }, propertyName, new List<PropertyAccessorModel>()));
            }

        var classFile = new ObjectFileModel<ClassModel>(
            @class,
            @class.UsingDirectives,
            @class.Name,
            directory,
            "cs"
            );

        _artifactGenerationStrategyFactory.CreateFor(classFile);

    }

    public void UnitTestCreateFor(string name, string directory)
    {
        _logger.LogInformation("Create Unit Test for {name}", name);
    }
}



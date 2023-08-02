// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Artifacts.Files;
using Endpoint.Core.Models.Syntax;
using Endpoint.Core.Models.Syntax.Classes;
using Endpoint.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Xunit;

namespace Endpoint.UnitTests;

public class ClassObjectFileArtifactGenerationStrategyTests
{
    public ClassObjectFileArtifactGenerationStrategyTests()
    {

    }

    [Fact]
    public void Test()
    {
        var services = new ServiceCollection();

        services.AddLogging();

        services.AddCoreServices<Marker>();

        var container = services.BuildServiceProvider();

        var sut = container.GetRequiredService<IArtifactGenerator>();

        var classModel = new ClassModel("Foo");

        var objectFileModel = new ObjectFileModel<ClassModel>(classModel, new List<UsingDirectiveModel>() {

            new UsingDirectiveModel() { Name = "Sytem" }
        }, "Foo", "directory", "extension");

        sut.CreateFor(objectFileModel);
    }
}


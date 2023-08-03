// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Syntax;
using Endpoint.Core.Syntax.Classes;
using Endpoint.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Endpoint.UnitTests;

public class ClassObjectFileArtifactGenerationStrategyTests
{
    public ClassObjectFileArtifactGenerationStrategyTests()
    {

    }

    [Fact]
    public async Task Test()
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

        await sut.CreateAsync(objectFileModel);
    }
}


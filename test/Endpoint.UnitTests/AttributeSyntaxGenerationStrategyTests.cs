// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Models.Syntax.Attributes;
using Endpoint.Core.Models.Syntax.Attributes.Strategies;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Endpoint.UnitTests;

public class AttributeSyntaxGenerationStrategyTests
{

    [Fact]
    public void CreateShould_ReturnAppropiateAttribute()
    {
        var services = new ServiceCollection();

        services.AddLogging();

        services.AddCoreServices();

        var container = services.BuildServiceProvider();


        var sut = ActivatorUtilities.CreateInstance<AttributeSyntaxGenerationStrategy>(container);

        Assert.NotNull(sut);

    }

    [Fact]
    public void CreateShould_ReturnApiControllerAttribute()
    {
        var services = new ServiceCollection();

        services.AddLogging();

        services.AddCoreServices();

        var container = services.BuildServiceProvider();


        var sut = ActivatorUtilities.CreateInstance<AttributeSyntaxGenerationStrategy>(container);

        var model = new AttributeModel()
        {
            Name = "ApiController"
        };

        var result = sut.Create(model);

        Assert.Equal("[ApiController]", result);

    }
}


// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Syntax.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Endpoint.Core.UnitTests.AttributeSyntaxGenerationStrategy;

using AttributeSyntaxGenerationStrategy = Syntax.Attributes.Strategies.AttributeSyntaxGenerationStrategy;

public class CreateShould
{
    [Fact]
    public void CreateExpectedSyntax_GivenValidModel()
    {
        // ARRANGE

        var services = new ServiceCollection();

        services.AddCoreServices();

        services.AddLogging();

        var container = services.BuildServiceProvider();

        var expected = "[Fact]";


        // ACT

        var sut = ActivatorUtilities.CreateInstance<AttributeSyntaxGenerationStrategy>(container);


        // ASSERT

        var result = sut.Create(new AttributeModel() { Name = "Fact" });

        Assert.Equal(expected, result);
    }

}



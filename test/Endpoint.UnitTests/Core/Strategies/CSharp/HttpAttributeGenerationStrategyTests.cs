// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.


using Endpoint.Core.Syntax.Attributes;
using Endpoint.Core.Syntax.Attributes.Strategies;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;


namespace Endpoint.UnitTests.Core.Strategies.CSharp;

public class HttpAttributeGenerationStrategyTests
{
    [Fact]
    public async Task ShouldCreateAttribute()
    {
        var expected = new string[1] { "[HttpGet(Name = \"getCharacters\")]" };

        var model = new AttributeModel(AttributeType.Http, "HttpGet", new()
        {
            { "Name", "getCharacters" }
        });

        var services = new ServiceCollection();

        services.AddLogging();

        services.AddCoreServices();

        var mockLogger = new Mock<ILogger<AttributeSyntaxGenerationStrategy>>().Object;

        var sut = new AttributeSyntaxGenerationStrategy(services.BuildServiceProvider(), mockLogger);

        var actual = await sut.GenerateAsync(default, model);

        Assert.Equal(string.Join(Environment.NewLine, expected), actual);
    }
}



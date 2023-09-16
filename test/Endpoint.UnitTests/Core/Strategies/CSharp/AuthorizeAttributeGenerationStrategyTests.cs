// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.


using Endpoint.Core.Syntax.Attributes;
using Endpoint.Core.Syntax.Attributes.Strategies;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;


namespace Endpoint.UnitTests.Core.Strategies.CSharp;

public class AuthorizeAttributeGenerationStrategyTests
{
    [Fact]
    public async Task ShouldCreateAuthorizeAttribute()
    {
        var expected = "[Authorize]";

        var services = new ServiceCollection();

        services.AddLogging();

        services.AddCoreServices();

        var mockLogger = new Mock<ILogger<AttributeSyntaxGenerationStrategy>>().Object;

        var sut = new AttributeSyntaxGenerationStrategy(mockLogger);

        var actual = await sut.GenerateAsync(default, new AttributeModel(AttributeType.Authorize, "Authorize", new Dictionary<string, string>()));

        Assert.Equal(expected, actual);
    }
}



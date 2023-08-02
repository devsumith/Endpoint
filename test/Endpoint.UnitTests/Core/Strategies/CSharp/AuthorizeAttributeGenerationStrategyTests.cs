// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Strategies.CSharp.Attributes;
using Endpoint.Core.Syntax.Attributes;
using Endpoint.Core.Syntax.Attributes.Strategies;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Endpoint.UnitTests.Core.Strategies.CSharp
{
    public class AuthorizeAttributeGenerationStrategyTests
    {
        [Fact]
        public void ShouldCreateAuthorizeAttribute()
        {
            var expected = "[Authorize]";

            var services = new ServiceCollection();

            services.AddLogging();

            services.AddCoreServices();

            var mockLogger = new Mock<ILogger<AttributeSyntaxGenerationStrategy>>().Object;

            var sut = new AttributeSyntaxGenerationStrategy(services.BuildServiceProvider(), mockLogger);

            var actual = sut.Create(new AttributeModel(AttributeType.Authorize, "Authorize", new System.Collections.Generic.Dictionary<string, string>()));

            Assert.Equal(expected, actual);
        }
    }
}


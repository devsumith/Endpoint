// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Castle.Core.Logging;
using Endpoint.Core.Models.Syntax.Attributes;
using Endpoint.Core.Models.Syntax.Attributes.Strategies;
using Endpoint.Core.Strategies.CSharp.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace Endpoint.UnitTests.Core.Strategies.CSharp
{
    public class HttpAttributeGenerationStrategyTests
    {
        [Fact]
        public void ShouldCreateAttribute()
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

            var actual = sut.Create(model);

            Assert.Equal(string.Join(Environment.NewLine, expected), actual);
        }
    }
}


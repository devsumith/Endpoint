using Endpoint.Core.Models.Syntax;
using Endpoint.Core.Strategies.CSharp.Attributes;
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

            var model = new AttributeModel(AttributeType.Http, "HttpGet", new ()
            {
                { "Name", "getCharacters" }
            });

            var sut = new HttpAttributeGenerationStrategy();

            var actual = sut.Create(model);

            Assert.Equal(string.Join(Environment.NewLine, expected), actual);
        }
    }
}

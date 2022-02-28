using Endpoint.Core.Models;
using Endpoint.Core.Strategies.CSharp.Attributes;
using System.Collections.Generic;
using Xunit;

namespace Endpoint.UnitTests.Core.Strategies.CSharp
{
    public class HttpAttributeGenerationStrategyTests
    {
        [Fact]
        public void ShouldCreateHttpGetAttribute()
        {
            var expected = new string[1] { "[HttpGet(Name = \"getCharacters\")]" };

            var properties = new Dictionary<string, string>();

            properties.Add("Name", "getCharacters");

            var model = new AttributeModel(AttributeType.Http, "HttpGet", properties);

            var sut = new HttpAttributeGenerationStrategy();

            var actual = sut.Create(model);

            Assert.Equal(expected, actual);
        }
    }
}

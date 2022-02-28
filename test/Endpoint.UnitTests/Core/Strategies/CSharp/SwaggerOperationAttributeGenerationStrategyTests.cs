using Endpoint.Core.Models;
using Endpoint.Core.Strategies.CSharp.Attributes;
using Xunit;

namespace Endpoint.UnitTests.Core.Strategies.CSharp
{
    public class SwaggerOperationAttributeGenerationStrategyTests
    {
        [Fact]
        public void ShouldCreateAttribute()
        {
            var expected = new string[4] {
                "[SwaggerOperation(",
                "    Summary = \"Get Show By Id.\",",
                "    Description = \"Get Show By Id.\"",
                ")]"
            };

            var sut = new SwaggerOperationAttributeGenerationStrategy();

            var model = new AttributeModel(AttributeType.SwaggerOperation, "SwaggerOperation", new()
            {
                { "Summary", "Get Show By Id." },
                { "Description", "Get Show By Id." },
            });

            var actual = sut.Create(model);

            for(var i = 0; i < actual.Length; i++)
            {
                Assert.Equal(expected[i], actual[i]);
            }

            Assert.Equal(expected, actual);
        }
    }
}

using Endpoint.Core.Models.Syntax;
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

            Assert.Equal(expected, actual);
        }
    }
}

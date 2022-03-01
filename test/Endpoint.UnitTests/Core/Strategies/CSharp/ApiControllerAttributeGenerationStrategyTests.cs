using Endpoint.Core.Strategies.CSharp.Attributes;
using Xunit;

namespace Endpoint.UnitTests.Core.Strategies.CSharp
{
    public class ApiControllerAttributeGenerationStrategyTests
    {
        [Fact]
        public void ShouldCreateAttribute()
        {
            var expected = new string[1] { "[ApiController]" };

            var sut = new ApiControllerAttributeGenerationStrategy();

            var actual = sut.Create(default);

            Assert.Equal(expected, actual);
        }
    }
}

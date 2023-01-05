using Endpoint.Core.Strategies.CSharp.Attributes;
using Xunit;

namespace Endpoint.UnitTests.Core.Strategies.CSharp
{
    public class AuthorizeAttributeGenerationStrategyTests
    {
        [Fact]
        public void ShouldCreateAuthorizeAttribute()
        {
            var expected = "[Authorize]";

            var sut = new AuthorizeAttributeGenerationStrategy();

            var actual = sut.Create(default);

            Assert.Equal(expected, actual);
        }
    }
}

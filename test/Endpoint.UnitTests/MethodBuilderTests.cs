


using Endpoint.Application.Builders;
using Xunit;

namespace Endpoint.UnitTests
{
    public class MethodBuilderTests
    {
        [Fact]
        public async void Constructor()
        {
            Setup();

            var sut = CreateMethodBuilder();
        }

        private void Setup()
        {

        }

        private MethodBuilder CreateMethodBuilder()
        {
            return new();
        }
    }
}

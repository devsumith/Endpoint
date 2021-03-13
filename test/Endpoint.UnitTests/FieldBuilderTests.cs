using Endpoint.Application.Builders;
using Xunit;

namespace Endpoint.UnitTests
{
    public class FieldBuilderTests
    {
        [Fact]
        public async void Constructor()
        {
            Setup();

            var sut = CreateFieldBuilder();
        }

        private void Setup()
        {

        }

        private FieldBuilder CreateFieldBuilder()
        {
            return new ();
        }
    }
}

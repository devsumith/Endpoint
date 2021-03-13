using Endpoint.Application.Builders;
using System.Threading.Tasks;
using Xunit;


namespace Endpoint.UnitTests
{
    public class AttributeBuilderTests
    {

        [Fact]
        public async void Constructor()
        {
            Setup();

            var sut = CreateAttributeBuilder();
        }

        [Fact]
        public void Basic()
        {
            var expected = "[Fact]";

            Setup();

            var sut = CreateAttributeBuilder();

            var actual = sut
                .WithName("Fact")
                .Build();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ProducesResponseTypeInternalServerError()
        {
            var expected = "        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]";

            var actual = AttributeBuilder.WithProducesResponseType(System.Net.HttpStatusCode.InternalServerError, indent: 2);

            Assert.Equal(expected, actual);
        }

        private void Setup()
        {

        }

        private AttributeBuilder CreateAttributeBuilder()
        {
            return new ();
        }
    }
}

using Endpoint.Core.Builders;
using Endpoint.Core.Enums;
using System.Net;
using Xunit;

namespace Endpoint.UnitTests
{
    public class AttributeBuilderTests
    {

        [Fact]
        public void Constructor()
        {

            var sut = new GenericAttributeGenerationStrategy();
        }

        [Fact]
        public void Basic()
        {
            var expected = "[Fact]";

            var actual = new GenericAttributeGenerationStrategy()
                .WithName("Fact")
                .Build();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ProducesResponseTypeInternalServerError()
        {
            var expected = "        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]";

            var actual = GenericAttributeGenerationStrategy.WithProducesResponseType(HttpStatusCode.InternalServerError, indent: 2);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ProducesResponseTypeOK()
        {
            var expected = "        [ProducesResponseType(typeof(GetCustomerById.Response), (int)HttpStatusCode.OK)]";

            var actual = GenericAttributeGenerationStrategy.WithProducesResponseType(HttpStatusCode.OK, type: "GetCustomerById.Response", indent: 2);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void HttpGet()
        {
            var expected = "        [HttpGet(\"{customerId}\", Name = \"GetCustomerByIdRoute\")]";

            var actual = GenericAttributeGenerationStrategy.WithHttp(HttpVerbs.Get, "{customerId}", "GetCustomerByIdRoute", indent: 2);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void HttpGetSimple()
        {
            var expected = "        [HttpGet(Name = \"GetCustomersRoute\")]";

            var actual = GenericAttributeGenerationStrategy.WithHttp(HttpVerbs.Get, routeName: "GetCustomersRoute", indent: 2);

            Assert.Equal(expected, actual);
        }
    }
}

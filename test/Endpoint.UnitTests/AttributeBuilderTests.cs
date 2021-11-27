using Endpoint.Application.Builders;
using Endpoint.Application.Enums;
using System.Net;
using Xunit;

namespace Endpoint.UnitTests
{
    public class AttributeBuilderTests
    {

        [Fact]
        public void Constructor()
        {

            var sut = new AttributeBuilder();
        }

        [Fact]
        public void Basic()
        {
            var expected = "[Fact]";

            var actual = new AttributeBuilder()
                .WithName("Fact")
                .Build();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ProducesResponseTypeInternalServerError()
        {
            var expected = "        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]";

            var actual = AttributeBuilder.WithProducesResponseType(HttpStatusCode.InternalServerError, indent: 2);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ProducesResponseTypeOK()
        {
            var expected = "        [ProducesResponseType(typeof(GetCustomerById.Response), (int)HttpStatusCode.OK)]";

            var actual = AttributeBuilder.WithProducesResponseType(HttpStatusCode.OK, type: "GetCustomerById.Response", indent: 2);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void HttpGet()
        {
            var expected = "        [HttpGet(\"{customerId}\", Name = \"GetCustomerByIdRoute\")]";

            var actual = AttributeBuilder.WithHttp(HttpVerbs.Get, "{customerId}", "GetCustomerByIdRoute", indent: 2);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void HttpGetSimple()
        {
            var expected = "        [HttpGet(Name = \"GetCustomersRoute\")]";

            var actual = AttributeBuilder.WithHttp(HttpVerbs.Get, routeName: "GetCustomersRoute", indent: 2);

            Assert.Equal(expected, actual);
        }
    }
}

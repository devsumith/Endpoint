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

        public void BasicSignature()
        {
            var expected = "        public async Task<ActionResult<GetCustomerById.Response>> GetById([FromRoute]GetCustomerById.Request request)";

            var actual = new MethodSignatureBuilder()
                
                .Build();

            Assert.Equal(expected, actual);

        }
        [Fact]
        public void ApiEndpoint()
        {
            var expected = new string[]
            {
                "        [Authorize]",
                "        [HttpGet(\"{customerId}\", Name = \"GetCustomerByIdRoute\")]",
                "        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]",
                "        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]",
                "        [ProducesResponseType(typeof(GetCustomerById.Response), (int)HttpStatusCode.OK)]",
                "        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]",
                "        public async Task<ActionResult<GetCustomerById.Response>> GetById([FromRoute]GetCustomerById.Request request)",
                "        {",
                "            var response = await _mediator.Send(request);",
                "",
                "            if (response.Customer == null)",
                "            {",
                "                return new NotFoundObjectResult(request.CustomerId);",
                "            }",
                "",
                "            return response;",
                "        }"
            };
            Setup();

            var sut = CreateMethodBuilder();

            var actual = sut.Build();

            Assert.Equal(expected, actual);

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

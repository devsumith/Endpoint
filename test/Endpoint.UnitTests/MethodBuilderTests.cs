using Endpoint.Application.Builders;
using Endpoint.Application.Enums;
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

        [Fact]
        public void BasicSignature()
        {
            var expected = "        public void Get()";

            var actual = new MethodSignatureBuilder()
                .WithEndpointType(EndpointType.Get)
                .WithIndent(2)
                .Build();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BasicAsyncSignature()
        {
            var expected = "        public async Task Get()";

            var actual = new MethodSignatureBuilder()
                .WithEndpointType(EndpointType.Get)
                .WithAsync(true)
                .WithReturnType("Task")
                .WithIndent(2)
                .Build();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BasicAsyncSignatureWithParams()
        {
            var expected = "        public async Task Create([FromBody]CreateCustomer.Request request)";

            var actual = new MethodSignatureBuilder()
                .WithEndpointType(EndpointType.Create)
                .WithParameter("[FromBody]CreateCustomer.Request request")
                .WithAsync(true)
                .WithReturnType("Task")
                .WithIndent(2)
                .Build();

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void Signature()
        {
            var expected = "        public async Task<ActionResult<GetCustomerById.Response>> GetById([FromRoute]GetCustomerById.Request request)";

            var actual = new MethodSignatureBuilder()
                .WithEndpointType(EndpointType.GetById)
                .WithParameter(new ParameterBuilder("GetCustomerById.Request", "request").WithFrom(From.Route).Build())
                .WithAsync(true)
                .WithReturnType(TypeBuilder.WithActionResult("GetCustomerById.Response"))
                .WithIndent(2)
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

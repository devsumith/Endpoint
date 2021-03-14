using Endpoint.Application.Builders;
using Endpoint.Application.Builders.CSharp;
using Endpoint.Application.Services;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Endpoint.UnitTests
{
    public class ClassBuilderTests
    {


        [Fact]
        public void Constructor()
        {

            var sut = new ClassBuilder("CustomerController", null, null);
        }


        [Fact]
        public void Controller()
        {
            var context = new Context();

            var expected = new List<string> {
                "using System.Net;",
                "using System.Threading.Tasks;",
                "",
                "namespace CustomerService.Api.Controllers",
                "{",
                "    [ApiController]",
                "    [Route(\"api/[controller]\")]",
                "    public class CustomerController",
                "    {",
                "        private readonly IMediator _mediator;",
                "",
                "        public CustomerController(IMediator mediator)",
                "",
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
                "        }",
                "    }",
                "}"
            }.ToArray();

            new ClassBuilder("CustomerController", context, Mock.Of<IFileSystem>())
                .WithUsing("System.Net")
                .WithUsing("System.Threading.Tasks")
                .WithNamespace("CustomerService.Api.Controllers")
                .WithAttribute(new AttributeBuilder().WithName("ApiController").Build())
                .WithAttribute(new AttributeBuilder().WithName("Route").WithParam("\"api/[controller]\"").Build())
                .WithDependency("IMediator", "mediator")
                .Build();

            var actual = context.ElementAt(0).Value;

            for(var i = 0; i < expected.Length; i++)
            {
                Assert.Equal(actual[i], expected[i]);
            }
        }

    }
}

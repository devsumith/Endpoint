
using Endpoint.Application.Builders.CSharp;
using Xunit;

namespace Endpoint.UnitTests
{
    public class ConstructorBuilderTests
    {

        [Fact]
        public async void Constructor()
        {
            var sut = new ConstructorBuilder("Customer");
        }

        [Fact]
        public void Basic()
        {
            var expected = new string[] { "    Customer()" };

            var sut = new ConstructorBuilder("Customer");

            var actual = sut.Build();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BasicWithDependency()
        {
            var expected = new string[] { 
                "CustomerController(IMediator mediator)", 
                "    => _mediator = mediator;" 
            };

            var sut = new ConstructorBuilder("CustomerController")
                .WithDependency("IMediator", "mediator");

            var actual = sut.Build();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BasicWithDependencies()
        {
            var expected = new string[] { "    CustomerController(IMediator mediator, IHttpContextAccessor httpContextAccessor)" };

            var sut = new ConstructorBuilder("CustomerController")
                .WithDependency("IMediator", "mediator")
                .WithDependency("IHttpContextAccessor", "httpContextAccessor");

            var actual = sut.Build();

            Assert.Equal(expected, actual);
        }

        private void Setup(string databaseName)
        {

        }


    }
}

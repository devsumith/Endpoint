using Endpoint.Application.Builders;
using Endpoint.Application.Services;
using Moq;
using System.Linq;
using Xunit;

namespace Endpoint.UnitTests
{
    public class CreateBuilderTests
    {
        [Fact]
        private void ShouldWork()
        {
            var context = new Context();

            new CreateBuilder(context, Mock.Of<IFileSystem>())
                .WithDirectory("")
                .WithDbContext("CustomerServiceDbContext")
                .WithNamespace("CustomerService.Application.Features")
                .WithApplicationNamespace("CustomerService.Application")
                .WithDomainNamespace("CustomerService.Domain")
                .WithEntity("Customer")
                .Build();

            var files = context.ElementAt(0).Value;

            Assert.NotNull(files);
        }
    }
}

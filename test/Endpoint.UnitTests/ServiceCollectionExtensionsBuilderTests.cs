using Endpoint.Application.Builders;
using Xunit;
using static Endpoint.Application.Builders.BuilderFactory;

namespace Endpoint.UnitTests
{
    public class ServiceCollectionExtensionsBuilderTests
    {
        [Fact]
        public async void Constructor()
        {
            Setup();

            _ = CreateServiceCollectionExtensionsBuilder();
        }

        [Fact]
        public async void Build()
        {
            Setup();

            var sut = CreateServiceCollectionExtensionsBuilder();

            sut.SetRootNamespace("ContactService")
                .SetDirectory(@"C:\Projects\ContactService\src\ContactService.Api\Extensions")
                .Build();
        }

        private void Setup()
        {

        }

        private static ServiceCollectionExtensionsBuilder CreateServiceCollectionExtensionsBuilder()
            => Create((c, tp, tl, f) => new ServiceCollectionExtensionsBuilder(c, tp, tl, f));
    }
}

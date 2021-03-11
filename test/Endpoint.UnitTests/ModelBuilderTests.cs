using Endpoint.Cli.Builders;
using Xunit;
using static Endpoint.Cli.Builders.BuilderFactory;

namespace Endpoint.UnitTests
{
    public class ModelBuilderTests
    {
        [Fact]
        public async void Constructor()
        {
            Setup();

            var sut = CreateModelBuilder();
        }

        [Fact]
        public async void Build()
        {
            Setup();

            var sut = CreateModelBuilder();

            sut.SetRootNamespace("ContactService")
                .SetNamespace("ContactService.Api.Models")
                .SetDirectory(@"C:\Projects\ContactService\src\ContactService.Api\Models")
                .SetEntityName("Contact")
                .Build();
        }

        private void Setup()
        {

        }

        private static ModelBuilder CreateModelBuilder()
            => Create((c,  tp, tl, f) => new ModelBuilder(c, tp, tl, f));
    }
}

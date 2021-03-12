using Endpoint.Application.Builders;
using Xunit;
using static Endpoint.Application.Builders.BuilderFactory;

namespace Endpoint.UnitTests
{
    public class ControllerBuilderTests
    {
        [Fact]
        public async void Constructor()
        {
            Setup();

            var sut = CreateControllerBuilder();
        }

        [Fact]
        public async void Build()
        {
            Setup();

            var sut = CreateControllerBuilder();

            sut.SetResource("Client")
                .SetRootNamespace("UnitTest")
                .SetDirectory(@"C:\Projects\UnitTests")
                .Build();
        }

        private void Setup()
        {

        }

        private static ControllerBuilder CreateControllerBuilder()
            => Create((c, tp, tl, f) => new ControllerBuilder(c, tp, tl, f));
    }
}

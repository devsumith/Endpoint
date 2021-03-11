using Endpoint.Cli.Builders;
using Xunit;
using static Endpoint.Cli.Builders.BuilderFactory;

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

            sut.SetResourceName("Client")
                .SetRootNamespace("UnitTest")
                .SetDirectory(@"C:\Projects\UnitTests")
                .Build();
        }

        private void Setup()
        {

        }

        private static ControllerBuilder CreateControllerBuilder()
            => Create("cli", (c, t, tp, tl, f, n) => new ControllerBuilder(c, t, tp, tl, f, n));
    }
}

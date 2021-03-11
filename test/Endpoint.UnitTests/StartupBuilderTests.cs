using Endpoint.Cli.Builders;
using Xunit;
using static Endpoint.Cli.Builders.BuilderFactory;

namespace Endpoint.UnitTests
{
    public class StartupBuilderTests
    {
        [Fact]
        public async void Constructor()
        {
            Setup();

            var sut = CreateStartupBuilder();
        }

        [Fact]
        public async void Build()
        {
            Setup();

            var sut = CreateStartupBuilder();

            sut.SetRootNamespace("")
                .SetDirectory(@"")
                .Build();
        }

        private void Setup()
        {

        }

        private static StartupBuilder CreateStartupBuilder()
            => Create("cli", (c, t, tp, tl, f, n) => new StartupBuilder(c, t, tp, tl, f, n));
    }
}

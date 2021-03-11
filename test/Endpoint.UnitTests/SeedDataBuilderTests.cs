using Endpoint.Cli.Builders;
using Xunit;
using static Endpoint.Cli.Builders.BuilderFactory;

namespace Endpoint.UnitTests
{
    public class SeedDataBuilderTests
    {
        [Fact]
        public async void Constructor()
        {
            Setup();

            var sut = CreateSeedDataBuilder();
        }

        [Fact]
        public async void Build()
        {
            Setup();

            var sut = CreateSeedDataBuilder();

            sut.SetRootNamespace("")
                .SetDirectory(@"")
                .Build();
        }

        private void Setup()
        {

        }

        private static SeedDataBuilder CreateSeedDataBuilder()
            => Create("cli", (c, t, tp, tl, f, n) => new SeedDataBuilder(c, t, tp, tl, f, n));
    }
}

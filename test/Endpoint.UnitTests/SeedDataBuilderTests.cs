using Endpoint.Application.Builders;
using Xunit;
using static Endpoint.Application.Builders.BuilderFactory;

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
            => Create((c, tp, tl, f) => new SeedDataBuilder(c, tp, tl, f));
    }
}

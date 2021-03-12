using Endpoint.Application.Builders;
using Xunit;
using static Endpoint.Application.Builders.BuilderFactory;

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
            => Create((c,  tp, tl, f) => new StartupBuilder(c, tp, tl, f));
    }
}

using Endpoint.Cli.Builders;
using Xunit;
using static Endpoint.Cli.Builders.BuilderFactory;

namespace Endpoint.UnitTests
{
    public class DependenciesBuilderTests
    {
        [Fact]
        public async void Constructor()
        {
            Setup();

            var sut = CreateDependenciesBuilder();
        }

        [Fact]
        public async void Build()
        {
            Setup();

            var sut = CreateDependenciesBuilder();

            sut.SetRootNamespace("")
                .SetDirectory(@"")
                .Build();
        }

        private void Setup()
        {

        }

        private static DependenciesBuilder CreateDependenciesBuilder()
            => Create("cli", (c, t, tp, tl, f, n) => new DependenciesBuilder(c, t, tp, tl, f, n));
    }
}

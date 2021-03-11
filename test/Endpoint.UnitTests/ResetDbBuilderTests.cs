using Endpoint.Cli.Builders;
using Xunit;
using static Endpoint.Cli.Builders.BuilderFactory;

namespace Endpoint.UnitTests
{
    public class ResetDbBuilderTests
    {
        [Fact]
        public async void Constructor()
        {
            Setup();

            var sut = CreateResetDbBuilder();
        }

        [Fact]
        public async void Build()
        {
            Setup();

            var sut = CreateResetDbBuilder();

            sut.SetRootNamespace("")
                .SetDirectory(@"")
                .Build();
        }

        private void Setup()
        {

        }

        private static ResetDbBuilder CreateResetDbBuilder()
            => Create("cli", (c, t, tp, tl, f, n) => new ResetDbBuilder(c, t, tp, tl, f, n));
    }
}

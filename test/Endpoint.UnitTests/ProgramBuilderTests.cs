using Endpoint.Cli.Builders;
using Xunit;
using static Endpoint.Cli.Builders.BuilderFactory;

namespace Endpoint.UnitTests
{
    public class ProgramBuilderTests
    {
        [Fact]
        public async void Constructor()
        {
            Setup();

            var sut = CreateProgramBuilder();
        }

        [Fact]
        public async void Build()
        {
            Setup();

            var sut = CreateProgramBuilder();

            sut.SetRootNamespace("")
                .SetDirectory(@"")
                .Build();
        }

        private void Setup()
        {

        }

        private static ProgramBuilder CreateProgramBuilder()
            => Create("cli", (c, t, tp, tl, f, n) => new ProgramBuilder(c, t, tp, tl, f, n));
    }
}

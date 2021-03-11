using Endpoint.Cli.Builders;
using Xunit;
using static Endpoint.Cli.Builders.BuilderFactory;

namespace Endpoint.UnitTests
{
    public class CommandBuilderTests
    {
        [Fact]
        public async void Constructor()
        {
            Setup();

            var sut = CreateCommandBuilder();
        }

        [Fact]
        public async void Build()
        {
            Setup();

            var sut = CreateCommandBuilder();

            sut.SetRootNamespace("")
                .SetDirectory(@"")
                .Build();
        }

        private void Setup()
        {

        }

        private static CommandBuilder CreateCommandBuilder()
            => Create("cli", (c, t, tp, tl, f, n) => new CommandBuilder(c, t, tp, tl, f, n));
    }
}

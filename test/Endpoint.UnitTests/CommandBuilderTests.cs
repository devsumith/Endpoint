using Endpoint.Application.Builders;
using Xunit;
using static Endpoint.Application.Builders.BuilderFactory;

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
            => Create((c, tp, tl, f) => new CommandBuilder(c, tp, tl, f));
    }
}

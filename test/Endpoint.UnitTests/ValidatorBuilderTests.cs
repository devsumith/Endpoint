using Endpoint.Cli.Builders;
using Xunit;
using static Endpoint.Cli.Builders.BuilderFactory;

namespace Endpoint.UnitTests
{
    public class ValidatorBuilderTests
    {
        [Fact]
        public async void Constructor()
        {
            Setup();

            var sut = CreateValidatorBuilder();
        }

        [Fact]
        public async void Build()
        {
            Setup();

            var sut = CreateValidatorBuilder();

            sut.SetRootNamespace("")
                .SetDirectory(@"")
                .Build();
        }

        private void Setup()
        {

        }

        private static ValidatorBuilder CreateValidatorBuilder()
            => Create("cli", (c, t, tp, tl, f, n) => new ValidatorBuilder(c, t, tp, tl, f, n));
    }
}

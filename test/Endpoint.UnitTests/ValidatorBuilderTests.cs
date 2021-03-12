using Endpoint.Application.Builders;
using Xunit;
using static Endpoint.Application.Builders.BuilderFactory;

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
            => Create((c, tp, tl, f) => new ValidatorBuilder(c, tp, tl, f));
    }
}

using Endpoint.Cli.Builders;
using Xunit;
using static Endpoint.Cli.Builders.BuilderFactory;

namespace Endpoint.UnitTests
{
    public class ValidationBehaviorBuilderTests
    {
        [Fact]
        public async void Constructor()
        {
            Setup();

            var sut = CreateValidationBehaviorBuilder();
        }

        [Fact]
        public async void Build()
        {
            Setup();

            var sut = CreateValidationBehaviorBuilder();

            sut.SetRootNamespace("")
                .SetDirectory(@"")
                .Build();
        }

        private void Setup()
        {

        }

        private static ValidationBehaviorBuilder CreateValidationBehaviorBuilder()
            => Create("cli", (c, t, tp, tl, f, n) => new ValidationBehaviorBuilder(c, t, tp, tl, f, n));
    }
}

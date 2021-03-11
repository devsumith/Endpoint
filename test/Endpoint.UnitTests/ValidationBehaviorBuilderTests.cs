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

            sut.SetRootNamespace("ContactService")
                .SetDirectory(@"C:\Projects\ContactService\src\ContactService.Api\Behaviors")
                .Build();
        }

        private void Setup()
        {

        }

        private static ValidationBehaviorBuilder CreateValidationBehaviorBuilder()
            => Create((c, tp, tl, f) => new ValidationBehaviorBuilder(c, tp, tl, f));
    }
}

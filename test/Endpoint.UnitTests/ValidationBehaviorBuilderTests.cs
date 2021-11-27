using Endpoint.Application.Builders;
using Xunit;
using static Endpoint.Application.Builders.BuilderFactory;

namespace Endpoint.UnitTests
{
    public class ValidationBehaviorBuilderTests
    {
        [Fact]
        public void Constructor()
        {
            Setup();

            var sut = CreateValidationBehaviorBuilder();
        }

        [Fact]
        public void Build()
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

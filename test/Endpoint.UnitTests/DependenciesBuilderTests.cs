using Endpoint.Application.Builders;
using Xunit;
using static Endpoint.Application.Builders.BuilderFactory;

namespace Endpoint.UnitTests
{
    public class DependenciesBuilderTests
    {
        [Fact]
        public void Constructor()
        {
            Setup();

            var sut = CreateDependenciesBuilder();
        }

        [Fact]
        public void Build()
        {
            Setup();

            var sut = CreateDependenciesBuilder();

            sut.SetRootNamespace("ContactService")
                .SetDirectory(@"C:\Projects\ContactService\src\ContactService.Api")
                .Build();
        }

        private void Setup()
        {

        }

        private static DependenciesBuilder CreateDependenciesBuilder()
            => Create((c, tp, tl, f) => new DependenciesBuilder(c, tp, tl, f));
    }
}

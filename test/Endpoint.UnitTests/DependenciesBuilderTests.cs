using Endpoint.Cli.Builders;
using Xunit;
using static Endpoint.Cli.Builders.BuilderFactory;

namespace Endpoint.UnitTests
{
    public class DependenciesBuilderTests
    {
        [Fact]
        public async void Constructor()
        {
            Setup();

            var sut = CreateDependenciesBuilder();
        }

        [Fact]
        public async void Build()
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
            => Create((c,  tp, tl, f) => new DependenciesBuilder(c, tp, tl, f));
    }
}

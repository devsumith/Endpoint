using Endpoint.Application.Builders;
using Xunit;
using static Endpoint.Application.Builders.BuilderFactory;

namespace Endpoint.UnitTests
{
    public class DbContextBuilderTests
    {
        [Fact]
        public void Constructor()
        {
            Setup();

            var sut = CreateDbContextBuilder();
        }

        [Fact]
        public void Build()
        {
            Setup();

            var sut = CreateDbContextBuilder();

            sut.SetRootNamespace("ContactService")
                .SetDirectory(@"C:\Projects\ContactService\src\ContactService.Api\Data")
                .WithModel("Contact")
                .Build();
        }

        private void Setup()
        {

        }

        private static DbContextBuilder CreateDbContextBuilder()
            => Create((c, tp, tl, f) => new DbContextBuilder(c, tp, tl, f));
    }
}

using Endpoint.Application.Builders;
using Xunit;
using static Endpoint.Application.Builders.BuilderFactory;

namespace Endpoint.UnitTests
{
    public class QueryBuilderTests
    {
        [Fact]
        public async void Constructor()
        {
            Setup();

            var sut = CreateQueryBuilder();
        }

        [Fact]
        public async void Build()
        {
            Setup();

            var sut = CreateQueryBuilder();

            sut.SetRootNamespace("")
                .SetDirectory(@"")
                .Build();
        }

        private void Setup()
        {

        }

        private static QueryBuilder CreateQueryBuilder()
            => Create((c, tp, tl, f) => new QueryBuilder(c, tp, tl, f));
    }
}

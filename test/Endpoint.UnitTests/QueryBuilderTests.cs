using Endpoint.Cli.Builders;
using Xunit;
using static Endpoint.Cli.Builders.BuilderFactory;

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
            => Create("cli", (c, t, tp, tl, f, n) => new QueryBuilder(c, t, tp, tl, f, n));
    }
}

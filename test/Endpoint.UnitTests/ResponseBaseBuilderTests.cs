using Endpoint.Cli.Builders;
using Xunit;
using static Endpoint.Cli.Builders.BuilderFactory;

namespace Endpoint.UnitTests
{
    public class ResponseBaseBuilderTests
    {
        [Fact]
        public async void Constructor()
        {
            Setup();

            var sut = CreateResponseBaseBuilder();
        }

        [Fact]
        public async void Build()
        {
            Setup();

            var sut = CreateResponseBaseBuilder();

            sut.SetRootNamespace("")
                .SetDirectory(@"")
                .Build();
        }

        private void Setup()
        {

        }

        private static ResponseBaseBuilder CreateResponseBaseBuilder()
            => Create((c, tp, tl, f) => new ResponseBaseBuilder(c, tp, tl, f));
    }
}

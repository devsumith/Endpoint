using Endpoint.Application.Builders;
using Xunit;
using static Endpoint.Application.Builders.BuilderFactory;

namespace Endpoint.UnitTests
{
    public class ResetDbBuilderTests
    {
        [Fact]
        public async void Constructor()
        {
            Setup();

            var sut = CreateResetDbBuilder();
        }

        [Fact]
        public async void Build()
        {
            Setup();

            var sut = CreateResetDbBuilder();

            sut.SetRootNamespace("")
                .SetDirectory(@"")
                .Build();
        }

        private void Setup()
        {

        }

        private static ResetDbBuilder CreateResetDbBuilder()
            => Create((c, tp, tl, f) => new ResetDbBuilder(c, tp, tl, f));
    }
}

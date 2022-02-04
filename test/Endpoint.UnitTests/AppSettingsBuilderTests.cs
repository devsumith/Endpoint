using Endpoint.Application.Builders;
using Xunit;
using static Endpoint.Application.Builders.BuilderFactory;

namespace Endpoint.UnitTests
{
    public class AppSettingsBuilderTests
    {
        [Fact]
        public void Constructor()
        {
            Setup();

            var sut = CreateAppSettingsBuilder();
        }

        [Fact]
        public void Build()
        {
            Setup();

            var sut = CreateAppSettingsBuilder();

            sut.SetRootNamespace("")
                .SetDirectory(@"")
                .Build();
        }

        private void Setup()
        {

        }

        private static AppSettingsBuilder CreateAppSettingsBuilder()
            => Create((c, tp, tl, f) => new AppSettingsBuilder(c, tp, tl, f));
    }
}

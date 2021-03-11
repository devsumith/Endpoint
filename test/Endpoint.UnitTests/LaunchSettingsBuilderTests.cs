using Endpoint.Cli.Builders;
using Xunit;
using static Endpoint.Cli.Builders.BuilderFactory;

namespace Endpoint.UnitTests
{
    public class LaunchSettingsBuilderTests
    {
        [Fact]
        public async void Constructor()
        {
            Setup();

            var sut = CreateLaunchSettingsBuilder();
        }

        [Fact]
        public async void Build()
        {
            Setup();

            var sut = CreateLaunchSettingsBuilder();

            sut.SetRootNamespace("")
                .SetDirectory(@"")
                .Build();
        }

        private void Setup()
        {

        }

        private static LaunchSettingsBuilder CreateLaunchSettingsBuilder()
            => Create("cli", (c, t, tp, tl, f, n) => new LaunchSettingsBuilder(c, t, tp, tl, f, n));
    }
}

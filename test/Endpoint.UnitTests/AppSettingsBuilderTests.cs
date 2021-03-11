using Endpoint.Cli.Builders;
using Endpoint.Cli.ValueObjects;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static Endpoint.Cli.Builders.BuilderFactory;

namespace Endpoint.UnitTests
{
    public class AppSettingsBuilderTests
    {
        [Fact]
        public async void Constructor()
        {
            Setup();

            var sut = CreateAppSettingsBuilder();
        }

        [Fact]
        public async void Build()
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

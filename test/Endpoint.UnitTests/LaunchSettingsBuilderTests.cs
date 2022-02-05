/*using Endpoint.Application.Builders;
using Xunit;


namespace Endpoint.UnitTests
{
    public class LaunchSettingsBuilderTests
    {
        [Fact]
        public void Constructor()
        {
            Setup();

            var sut = CreateLaunchSettingsBuilder();
        }

        [Fact]
        public void Build()
        {
            Setup();

            var sut = CreateLaunchSettingsBuilder();

            sut.SetDirectory(@"C:\Projects\ContactService\src\ContactService.Api\Properties")
                .WithLaunchUrl("")
                .Build();
        }

        private void Setup()
        {

        }

        private static LaunchSettingsBuilder CreateLaunchSettingsBuilder()
            => Create((c, tp, tl, f) => new LaunchSettingsBuilder(c, tp, tl, f));
    }
}
*/
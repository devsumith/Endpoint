using Endpoint.Application.Builders;
using Xunit;
using static Endpoint.Application.Builders.BuilderFactory;

namespace Endpoint.UnitTests
{
    public class ProgramBuilderTests
    {
        [Fact]
        public void Constructor()
        {
            Setup();

            var sut = CreateProgramBuilder();
        }

        [Fact]
        public void Build()
        {
            Setup();

            var sut = CreateProgramBuilder();

            sut.SetRootNamespace("ContactService")
                .SetDirectory(@"C:\Projects\ContactService\src\ContactService.Api")
                .Build();
        }

        private void Setup()
        {

        }

        private static ProgramBuilder CreateProgramBuilder()
            => Create((c, tp, tl, f) => new ProgramBuilder(c, tp, tl, f));
    }
}

using Endpoint.Application.Builders;
using Endpoint.Application.Services;
using System.IO;
using Xunit;

namespace Endpoint.UnitTests
{
    public class NamespaceBuilderTests
    {
        [Fact]
        public void Constructor()
        {
            var context = new Context();

            var sut = new NamespaceBuilder("Avengers.Api.Features","Test", context, default);

            sut.Build();

            var expected = new string[4]
            {
                "namespace Avengers.Api.Features", "{","","}"
            };

            context.TryGetValue($"{Path.DirectorySeparatorChar}Test.cs", out string[] actual);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddUsings()
        {
            var sut = new NamespaceBuilder("Avengers.Api.Features", default, default, default)
                .WithUsing("System")
                .WithUsing("System.Linq");

            sut.Build();

            var expected = new string[7]
            {
                "using System;","using System.Linq;", "", "namespace Avengers.Api.Features", "{","","}"
            };

            //Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddClasses()
        {
            var sut = new NamespaceBuilder("Avengers.Api.Features", default, default, default)
                .WithUsing("System")
                .WithUsing("System.Linq")
                .WithClass(new ClassBuilder("Foo", default, null).Class);

            sut.Build();

            var expected = new string[9]
            {
                "using System;","using System.Linq;", "", "namespace Avengers.Api.Features", "{","","    public class Foo { }", "", "}"
            };

            //Assert.Equal(expected, actual);
        }

    }
}

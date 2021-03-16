using Endpoint.Application.Builders;
using Endpoint.Application.Enums;
using Endpoint.Application.ValueObjects;
using Xunit;

namespace Endpoint.UnitTests
{
    public class PropertyBuilderTests
    {
        [Fact]
        public async void Constructor()
        {
            Setup();

            var sut = CreatePropertyBuilder();
        }

        [Fact]
        public void Basic()
        {
            var expected = "    public DbSet<Customer> Customers { get; set; }";

            Setup();

            var sut = CreatePropertyBuilder();

            var entity = (Token)"Customer";

            var actual = sut
                .WithIndent(1)
                .WithType(new TypeBuilder()
                    .WithGenericType("DbSet", entity.PascalCase)
                    .Build())
                .WithAccessors(new AccessorsBuilder().Build())
                .WithName(entity.PascalCasePlural)
                .Build();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void NoAccessor()
        {
            var expected = "DbSet<Customer> Customers { get; }";

            Setup();

            var sut = CreatePropertyBuilder();

            var entity = (Token)"Customer";

            var actual = sut
                .WithType(new TypeBuilder()
                    .WithGenericType("DbSet", entity.PascalCase)
                    .Build())
                .WithAccessors(new AccessorsBuilder().WithGetterOnly().Build())
                .WithAccessModifier(AccessModifier.Inherited)
                .WithName(entity.PascalCasePlural)
                .Build();

            Assert.Equal(expected, actual);
        }

        private void Setup()
        {

        }

        private PropertyBuilder CreatePropertyBuilder()
        {
            return new();
        }
    }
}

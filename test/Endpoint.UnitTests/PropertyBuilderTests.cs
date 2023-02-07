// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Builders;
using Endpoint.Core.Enums;
using Endpoint.Core.Models.Syntax;
using Xunit;

namespace Endpoint.UnitTests
{
    public class PropertyBuilderTests
    {
        [Fact]
        public void Constructor()
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

            var entity = (SyntaxToken)"Customer";

            var actual = sut
                .WithIndent(1)
                .WithType(new TypeBuilder()
                    .WithGenericType("DbSet", entity.PascalCase())
                    .Build())
                .WithAccessors(new AccessorsBuilder().Build())
                .WithName(entity.PascalCasePlural())
                .Build();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void NoAccessor()
        {
            var expected = "DbSet<Customer> Customers { get; }";

            Setup();

            var sut = CreatePropertyBuilder();

            var entity = (SyntaxToken)"Customer";

            var actual = sut
                .WithType(new TypeBuilder()
                    .WithGenericType("DbSet", entity.PascalCase())
                    .Build())
                .WithAccessors(new AccessorsBuilder().WithGetterOnly().Build())
                .WithAccessModifier(AccessModifier.Inherited)
                .WithName(entity.PascalCasePlural())
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


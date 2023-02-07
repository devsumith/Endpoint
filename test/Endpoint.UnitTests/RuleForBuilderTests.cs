// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Builders;
using Xunit;

namespace Endpoint.UnitTests
{
    public class RuleForBuilderTests
    {
        [Fact]
        public void Constructor()
        {
            var sut = new RuleForBuilder();
        }

        [Fact]
        public void Basic()
        {
            var expected = "RuleFor(request => request.Customer).NotNull();";

            var sut = new RuleForBuilder().WithEntity("Customer").WithNotNull();

            var actual = sut
                .Build();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SetValidator()
        {
            var expected = "RuleFor(request => request.Customer).SetValidator(new CustomerValidator());";

            var sut = new RuleForBuilder().WithEntity("Customer").WithValidator();

            var actual = sut
                .Build();

            Assert.Equal(expected, actual);
        }
    }
}


using Endpoint.Core.Builders;
using Endpoint.Core.Enums;
using Endpoint.Core.Services;
using Endpoint.Core.Models.Syntax;
using Moq;
using System.Collections.Generic;
using Xunit;


namespace Endpoint.UnitTests
{
    public class ControllerBuilderTests
    {
        [Fact]
        public void Constructor()
        {

            var sut = new ControllerBuilder();
        }
    }
}

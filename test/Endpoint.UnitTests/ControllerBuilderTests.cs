using Endpoint.Application.Builders;
using Endpoint.Application.Enums;
using Endpoint.SharedKernal.Services;
using Endpoint.SharedKernal.ValueObjects;
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

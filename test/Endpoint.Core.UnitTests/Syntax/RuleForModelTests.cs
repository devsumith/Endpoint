namespace Endpoint.Core.UnitTests.Syntax
{
    using System;
    using Endpoint.Core.Syntax;
    using Endpoint.Core.Syntax.Properties;
    using Endpoint.Core.Syntax.Types;
    using Xunit;

    public class RuleForModelTests
    {
        private RuleForModel _testClass;
        private PropertyModel _property;

        public RuleForModelTests()
        {
            _property = new PropertyModel(new TypeModel("TestValue1785358663"), "TestValue563825375", new PropertyAccessorModel(PropertyAccessorType.Set));
            _testClass = new RuleForModel(_property);
        }

        [Fact]
        public void CanConstruct()
        {
            // Act
            var instance = new RuleForModel(_property);

            // Assert
            Assert.NotNull(instance);
        }

        [Fact]
        public void CannotConstructWithNullProperty()
        {
            Assert.Throws<ArgumentNullException>(() => new RuleForModel(default(PropertyModel)));
        }

        [Fact]
        public void PropertyIsInitializedCorrectly()
        {
            Assert.Same(_property, _testClass.Property);
        }

        [Fact]
        public void CanSetAndGetProperty()
        {
            // Arrange
            var testValue = new PropertyModel(new TypeModel("TestValue1766792680"), "TestValue658604556", new PropertyAccessorModel(PropertyAccessorType.Get));

            // Act
            _testClass.Property = testValue;

            // Assert
            Assert.Same(testValue, _testClass.Property);
        }
    }
}
namespace Endpoint.Core.UnitTests.Syntax
{
    using System;
    using Endpoint.Core.Syntax;
    using Endpoint.Core.Syntax.Properties;
    using Endpoint.Core.Syntax.Types;
    using Xunit;

    public class RuleForModelTests
    {
        private RuleForModel testClass;
        private PropertyModel property;

        public RuleForModelTests()
        {
            property = new PropertyModel(new TypeModel("TestValue1785358663"), "TestValue563825375", new PropertyAccessorModel(PropertyAccessorType.Set));
            testClass = new RuleForModel(property);
        }

        [Fact]
        public void CanConstruct()
        {
            // Act
            var instance = new RuleForModel(property);

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
            Assert.Same(property, testClass.Property);
        }

        [Fact]
        public void CanSetAndGetProperty()
        {
            // Arrange
            var testValue = new PropertyModel(new TypeModel("TestValue1766792680"), "TestValue658604556", new PropertyAccessorModel(PropertyAccessorType.Get));

            // Act
            testClass.Property = testValue;

            // Assert
            Assert.Same(testValue, testClass.Property);
        }
    }
}
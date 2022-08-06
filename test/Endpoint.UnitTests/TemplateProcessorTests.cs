using Endpoint.Core.Services;
using Xunit;

namespace Endpoint.UnitTests
{

    public class TemplateProcessorTests
    {
        internal record MyModel(string Title);

        [Fact]
        public void Process_ShouldRenderObject()
        {
            var template = new string[1] { "{{ Title }}" };

            var sut = new LiquidTemplateProcessor();

            var result = sut.Process(template, new MyModel("Foo"));

            Assert.Contains("Foo", result);

        }
    }
}

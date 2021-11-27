using Endpoint.Application.Builders;
using Endpoint.Application.Enums;
using Endpoint.Application.Services;
using Endpoint.Application.ValueObjects;
using Moq;
using System.Collections.Generic;
using Xunit;
using static Endpoint.Application.Builders.BuilderFactory;

namespace Endpoint.UnitTests
{
    public class ControllerBuilderTests
    {
        [Fact]
        public void Constructor()
        {
            Setup();

            var sut = CreateControllerBuilder();
        }

        [Fact]
        public void Build()
        {
            var expected = new List<string>
            {
                ""
            }.ToArray();

            var _entityName = "Foo";
            var _namespace = "CustomerService.Api";

            var getById = new MethodBuilder().WithEndpointType(EndpointType.GetById).WithResource(_entityName).WithAuthorize(false).Build();
            var get = new MethodBuilder().WithEndpointType(EndpointType.Get).WithResource(_entityName).WithAuthorize(false).Build();
            var create = new MethodBuilder().WithEndpointType(EndpointType.Create).WithResource(_entityName).WithAuthorize(false).Build();
            var update = new MethodBuilder().WithEndpointType(EndpointType.Update).WithResource(_entityName).WithAuthorize(false).Build();
            var remove = new MethodBuilder().WithEndpointType(EndpointType.Delete).WithResource(_entityName).WithAuthorize(false).Build();
            var page = new MethodBuilder().WithEndpointType(EndpointType.Page).WithResource(_entityName).WithAuthorize(false).Build();

            var actual = new ClassBuilder($"{((Token)_entityName).PascalCase}Controller", new Context(), Mock.Of<IFileSystem>())
                .WithUsing("System.Net")
                .WithUsing("System.Threading.Tasks")
                .WithUsing($"{_namespace}.Features")
                .WithUsing("MediatR")
                .WithUsing("Microsoft.AspNetCore.Mvc")
                .WithNamespace($"{_namespace}.Controllers")
                .WithAttribute(new AttributeBuilder().WithName("ApiController").Build())
                .WithAttribute(new AttributeBuilder().WithName("Route").WithParam("\"api/[controller]\"").Build())
                .WithDependency("IMediator", "mediator")
                .WithMethod(getById)
                .WithMethod(get)
                .WithMethod(create)
                .WithMethod(update)
                .WithMethod(remove)
                .WithMethod(page)
                .Class;

            Assert.Equal(actual, actual);
        }

        private void Setup()
        {

        }

        private static ControllerBuilder CreateControllerBuilder()
            => Create((c, tp, tl, f) => new ControllerBuilder(c, tp, tl, f));
    }
}

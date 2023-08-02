
using Endpoint.Core.Builders;
using Endpoint.Core.Syntax;
using Xunit;


namespace Endpoint.UnitTests;

public class FieldBuilderTests
{
    [Fact]
    public void Constructor()
    {
        var sut = new FieldBuilder("Test", "Test");
    }

    [Fact]
    public void Basic()
    {
        var expected = "private int _count;";

        var sut = new FieldBuilder("int", "count")
            .WithAccessModifier(AccessModifier.Private);

        var actual = sut.Build();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void BasicReadonly()
    {
        var expected = "private readonly int _count;";

        var sut = new FieldBuilder("int", "count")
            .WithReadonly()
            .WithAccessModifier(AccessModifier.Private);

        var actual = sut.Build();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void BasicPublic()
    {
        var expected = "public int count;";

        var sut = new FieldBuilder("int", "count")
            .WithAccessModifier(AccessModifier.Public);

        var actual = sut.Build();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Inherited()
    {
        var expected = "int count;";

        var sut = new FieldBuilder("int", "count");

        var actual = sut.Build();

        Assert.Equal(expected, actual);
    }
}


using MyNewProjectName.Grammars.Business;
using FluentAssertions;
using Xunit;

namespace MyNewProjectName.Tests.Grammars;
public class PostgresSyntaxFormatterTests
{
    private readonly PostgresSyntaxFormatter sut;

    public PostgresSyntaxFormatterTests()
    {
        sut = new PostgresSyntaxFormatter();
    }

    [Fact]
    public void FormatIdentifier_Should_WrapStringInDoubleQuotes_When_CalledWithValidString()
    {
        var columnName = "studentnumber";
        var postgresResultFormatter =  sut.FormatIdentifier(columnName);
        postgresResultFormatter.Should().Be("\"studentnumber\"");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public void GetParameterName_Should_ReturnParameterWithDollarPrefix_When_IndexIsProvided(int index)
    {
        var postgresResultFormatter = sut.GetParameterName(index);
        postgresResultFormatter.Should().Be($"${index}");
    }

    [Fact]
    public void ParametrStartIndex_Should_BeOne_When_YouWantToAccessTheDatabase()
    {
        var postgresResultFormatter = sut.ParameterStartIndex;
        postgresResultFormatter.Should().Be(1);
    }
}

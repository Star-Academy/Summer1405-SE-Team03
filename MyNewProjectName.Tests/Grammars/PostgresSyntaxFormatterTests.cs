using MyNewProjectName.Grammars.Business;
using FluentAssertions;
using Xunit;

namespace MyNewProjectName.Tests.Grammars;
public class PostgresSyntaxFormatterTests
{
    private readonly PostgresSyntaxFormatter _sut;

    public PostgresSyntaxFormatterTests()
    {
        _sut = new PostgresSyntaxFormatter();
    }

    [Fact]
    public void FormatIdentifier_Should_WrapStringInDoubleQuotes_When_CalledWithValidString()
    {
        var columnName = "studentnumber";
        var postgresResultFormatter =  _sut.FormatIdentifier(columnName);
        postgresResultFormatter.Should().Be("\"studentnumber\"");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public void GetParameterName_Should_ReturnParameterWithDollarPrefix_When_IndexIsProvided(int index)
    {
        var postgresResultFormatter = _sut.GetParameterName(index);
        postgresResultFormatter.Should().Be($"${index}");
    }

    [Fact]
    public void ParametrStartIndex_Should_BeOne_When_YouWantToAccessTheDatabase()
    {
        var postgresResultFormatter = _sut.ParameterStartIndex;
        postgresResultFormatter.Should().Be(1);
    }
}

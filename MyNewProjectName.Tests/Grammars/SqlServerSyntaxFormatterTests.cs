using MyNewProjectName.Grammars.Business;
using FluentAssertions;
using Xunit;

namespace MyNewProjectName.Tests.Grammars;

public class SqlServerSyntaxFormatterTests
{
    private readonly SqlServerSyntaxFormatter _sut;

    public SqlServerSyntaxFormatterTests()
    {
        _sut = new SqlServerSyntaxFormatter();
    }

    [Fact]
    public void FormatIdentifier_ShouldWrapStringInBrackets_WhenCalledWithValidString()
    {
        var columnName = "studentnumber";
        var result = _sut.FormatIdentifier(columnName);
        result.Should().Be("[studentnumber]");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public void GetParameterName_ShouldReturnParameterWithAtPrefixedAtP_WhenIndexIsProvided(int index)
    {
        var result = _sut.GetParameterName(index);
        result.Should().Be($"@p{index}");
    }

    [Fact]
    public void ParametrStartIndex_ShouldBeZero_WhenYouWantToAccessTheDatabase()
    {
        var result = _sut.ParameterStartIndex;
        result.Should().Be(0);
    }
}
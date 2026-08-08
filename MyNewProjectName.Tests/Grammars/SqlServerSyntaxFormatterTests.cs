using MyNewProjectName.Grammars.Business;
using FluentAssertions;
using Xunit;

namespace MyNewProjectName.Tests.Grammars;

public class SqlServerSyntaxFormatterTests
{
    private readonly SqlServerSyntaxFormatter sut;

    public SqlServerSyntaxFormatterTests()
    {
        sut = new SqlServerSyntaxFormatter();
    }

    [Fact]
    public void FormatIdentifier_Should_WrapStringInBrackets_When_CalledWithValidString()
    {
        var columnName = "studentnumber";
        var result = sut.FormatIdentifier(columnName);
        result.Should().Be("[studentnumber]");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public void GetParameterName_Should_ReturnParameterWithAtPrefixedAtP_When_IndexIsProvided(int index)
    {
        var result = sut.GetParameterName(index);
        result.Should().Be($"@p{index}");
    }

    [Fact]
    public void ParametrStartIndex_Should_BeZero_When_YouWantToAccessTheDatabase()
    {
        var result = sut.ParameterStartIndex;
        result.Should().Be(0);
    }
}
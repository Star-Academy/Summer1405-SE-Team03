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
        // Arrange
        var columnName = "studentnumber";

        // Act
        var result = _sut.FormatIdentifier(columnName);

        // Assert
        result.Should().Be("[studentnumber]");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public void GetParameterName_ShouldReturnParameterWithAtPrefixedAtP_WhenIndexIsProvided(int index)
    {
        // Arrange
        // Act
        var result = _sut.GetParameterName(index);

        // Assert
        result.Should().Be($"@p{index}");
    }

    [Fact]
    public void ParametrStartIndex_ShouldBeZero_WhenYouWantToAccessTheDatabase()
    {
        // Arrange
        // Act
        var result = _sut.ParameterStartIndex;

        // Assert
        result.Should().Be(0);
    }
}
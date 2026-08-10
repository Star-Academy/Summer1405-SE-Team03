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
    public void FormatIdentifier_ShouldWrapStringInDoubleQuotes_WhenCalledWithValidString()
    {
        // Arrange
        var columnName = "studentnumber";

        // Act
        var postgresResultFormatter = _sut.FormatIdentifier(columnName);

        // Assert
        postgresResultFormatter.Should().Be("\"studentnumber\"");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public void GetParameterName_ShouldReturnParameterWithDollarPrefix_WhenIndexIsProvided(int index)
    {
        // Arrange
        // Act
        var postgresResultFormatter = _sut.GetParameterName(index);

        // Assert
        postgresResultFormatter.Should().Be($"@{index}");
    }

    [Fact]
    public void ParametrStartIndex_ShouldBeOne_WhenYouWantToAccessTheDatabase()
    {
        // Arrange
        // Act
        var postgresResultFormatter = _sut.ParameterStartIndex;

        // Assert
        postgresResultFormatter.Should().Be(1);
    }
}
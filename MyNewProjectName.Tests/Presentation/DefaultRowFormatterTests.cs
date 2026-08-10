using System.Data;
using FluentAssertions;
using MyNewProjectName.Presentation.Business;
using NSubstitute;
using Xunit;

namespace MyNewProjectName.Tests.Presentation;

public class DefaultRowFormatterTests
{
    private readonly DefaultRowFormatter _sut;

    public DefaultRowFormatterTests()
    {
        _sut = new DefaultRowFormatter();
    }

    [Fact]
    public void FormatRow_ShouldReturnFormattedString_WhenSingleColumnIsProvided()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();
        reader.FieldCount.Returns(1);
        reader.GetName(0).Returns("Id");
        reader.GetValue(0).Returns(100);

        // Act
        var result = _sut.FormatRow(reader);

        // Assert
        result.Should().Be("Id: 100");
    }

    [Fact]
    public void FormatRow_ShouldReturnCommaSeparatedString_WhenMultipleColumnsAreProvided()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();
        reader.FieldCount.Returns(2);
        reader.GetName(0).Returns("Id");
        reader.GetValue(0).Returns(100);
        reader.GetName(1).Returns("Name");
        reader.GetValue(1).Returns("Ali");

        // Act
        var result = _sut.FormatRow(reader);

        // Assert
        result.Should().Be("Id: 100, Name: Ali");
    }

    [Fact]
    public void FormatRow_ShouldReturnEmptyValue_WhenValueIsDbNull()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();
        reader.FieldCount.Returns(1);
        reader.GetName(0).Returns("Age");
        reader.GetValue(0).Returns(System.DBNull.Value); 

        // Act
        var result = _sut.FormatRow(reader);

        // Assert
        result.Should().Be("Age: "); 
    }
}
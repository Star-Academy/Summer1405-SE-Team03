using System;
using FluentAssertions;
using MyNewProjectName.Compilers.Business;
using MyNewProjectName.Core;
using MyNewProjectName.Grammars.Abstractions;
using NSubstitute;
using Xunit;

namespace MyNewProjectName.Tests.Compilers;

public class FromClauseBuilderTests
{
    private readonly IDatabaseSpecificSyntaxFormatter _formatterSubstitute;
    private readonly FromClauseBuilder _sut;

    public FromClauseBuilderTests()
    {
        _formatterSubstitute = Substitute.For<IDatabaseSpecificSyntaxFormatter>();
        _sut = new FromClauseBuilder(_formatterSubstitute);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenFormatterIsNull()
    {
        // Arrange & Act
        var nullAction = () => new FromClauseBuilder(null!);
        
        // Assert
        nullAction.Should().Throw<ArgumentNullException>().WithParameterName("databaseSpecificSyntaxFormatter");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Build_ShouldThrowInvalidOperationException_WhenTableNameIsEmptyOrWhitespace(string invalidTableName)
    {
        // Arrange
        var invalidQuery = new Query();
        if (invalidTableName != null)
        {
            invalidQuery.From(invalidTableName);
        }

        // Act
        Action act = () => _sut.Build(invalidQuery);
        
        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("Table name cannot be null or empty.");
    }

    [Fact]
    public void Build_ShouldReturnFormattedString_WhenTableNameIsValid()
    {
        // Arrange
        var validQuery = new Query().From("student");
        _formatterSubstitute.FormatIdentifier("student").Returns("\"student\"");
        
        // Act
        var result = _sut.Build(validQuery);
        
        // Assert
        result.Should().Be(" FROM \"student\"");
        _formatterSubstitute.Received(1).FormatIdentifier("student");
    }
}
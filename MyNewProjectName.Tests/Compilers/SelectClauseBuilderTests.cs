using System;
using FluentAssertions;
using MyNewProjectName.Compilers.Business;
using MyNewProjectName.Core;
using MyNewProjectName.Grammars.Abstractions;
using NSubstitute;
using Xunit;

namespace MyNewProjectName.Tests.Compilers;

public class SelectClauseBuilderTests
{
    private readonly SelectClauseBuilder _sut;
    private readonly IDatabaseSpecificSyntaxFormatter _formatterSubstitute;

    public SelectClauseBuilderTests()
    {
        _formatterSubstitute = Substitute.For<IDatabaseSpecificSyntaxFormatter>();
        _sut = new SelectClauseBuilder(_formatterSubstitute);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenFormatterIsNull()
    {
        // Arrange & Act
        var nullAction = () => new SelectClauseBuilder(null!);
        
        // Assert
        nullAction.Should().Throw<ArgumentNullException>().WithParameterName("databaseSpecificSyntaxFormatter");
    }

    [Fact]
    public void Build_ShouldReturnSelectStar_WhenWeHaveEmptyColumn()
    {
        // Arrange
        var emptySelectClauseQuery = new Query();
        
        // Act
        var emptyResult = _sut.Build(emptySelectClauseQuery);
        
        // Assert
        emptyResult.Should().Be("SELECT * ");
    }

    [Fact]
    public void Build_ShouldReturnFormattedColumnsJoinedByComma_WhenColumnsAreProvided()
    {
        // Arrange
        var validSelectClauseQuery = new Query().Select("studentnumber", "firstname");
        _formatterSubstitute.FormatIdentifier("studentnumber").Returns("\"studentnumber\"");
        _formatterSubstitute.FormatIdentifier("firstname").Returns("\"firstname\"");
        
        // Act
        var selectClauseResult = _sut.Build(validSelectClauseQuery);
        
        // Assert
        selectClauseResult.Should().Be("SELECT \"studentnumber\", \"firstname\"");
        _formatterSubstitute.Received(1).FormatIdentifier("studentnumber");
        _formatterSubstitute.Received(1).FormatIdentifier("firstname");
    }
}
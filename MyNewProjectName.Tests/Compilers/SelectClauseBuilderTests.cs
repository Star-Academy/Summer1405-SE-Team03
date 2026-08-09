using System;
using FluentAssertions;
using MyNewProjectName.Compilers.Business;
using MyNewProjectName.Core;
using MyNewProjectName.Grammars.Abstractions;
using NSubstitute;
using Xunit;
using System.Runtime.Serialization;
using MyNewProjectName.Compilers.Business;

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
        //act
        var nullAction = () => new SelectClauseBuilder(null!);
        //assert
        nullAction.Should().Throw<ArgumentNullException>().WithParameterName("databaseSpecificSyntaxFormatter");
    }

    [Fact]
    public void Constructor_ShouldReturnOnlySelect_WhenWeHaveEmptyColumn()
    {
        //act
        var emptySelectClauseQuery = new Query();
        //assert
        var emptyResult = _sut.Build(emptySelectClauseQuery);
        //act
        emptyResult.Should().Be("SELECT * ");
        
    }

    [Fact]
    public void Build_ShouldReturnFormattedColumnsJoinedByComma_WhenColumnsAreProvided()
    {
        //arrange
        var validSelectClauseQuery = new Query().Select("studentnumber", "firstname");
        _formatterSubstitute.FormatIdentifier("studentnumber").Returns("\"studentnumber\"");
        _formatterSubstitute.FormatIdentifier("firstname").Returns("\"firstname\"");
        //act
        var selectClauseResult = _sut.Build(validSelectClauseQuery);
        //assert
        selectClauseResult.Should().Be("SELECT \"studentnumber\", \"firstname\"");
        _formatterSubstitute.Received(1).FormatIdentifier("studentnumber");
        _formatterSubstitute.Received(1).FormatIdentifier("firstname");
    }
}
using System;
using System.Collections.Generic;
using FluentAssertions;
using MyNewProjectName.Core;
using MyNewProjectName.Execution.Business;
using MyNewProjectName.Grammars.Abstractions;
using NSubstitute;
using Xunit;

namespace MyNewProjectName.Tests.Execution;

public class PostgresQueryParameterBinderTests
{
    private readonly PostgresQueryParameterBinder _sut;
    private readonly IDatabaseSpecificSyntaxFormatter _formatterSubstitute ;

    public PostgresQueryParameterBinderTests()
    {
        _formatterSubstitute = Substitute.For<IDatabaseSpecificSyntaxFormatter>();
        _sut = new PostgresQueryParameterBinder(_formatterSubstitute);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenFormatterIsNull()
    {
        //act
        Action act = () => new PostgresQueryParameterBinder(null!);

        //assert
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("databaseSpecificSyntaxFormatter");
    }

    [Fact]
    public void BindParameters_ShouldThrowArgumentNullException_WhenQueryIsNull()
    {
        //act
        Action act = () => _sut.BindParameters(null!);

        //assert
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("query");
    }

    [Fact]
    public void BindParameters_ShouldReturnEmptyList_WhenWhereConditionsAreEmpty()
    {
        //arrange
        var query = new Query();

        //act
        var result = _sut.BindParameters(query);

        //assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void BindParameters_ShouldReturnQueryParameters_WhenWhereConditionsAreProvided()
    {
        //arrange
        var query = new Query()
            .Where("age", 20)
            .Where("ismale", true);

        _formatterSubstitute.ParameterStartIndex.Returns(1);
        _formatterSubstitute.GetParameterName(1).Returns("$1");
        _formatterSubstitute.GetParameterName(2).Returns("$2");

        //act
        var result = _sut.BindParameters(query);

        //assert
        result.Should().HaveCount(2);
        
        result[0].Name.Should().Be("$1");
        result[0].Value.Should().Be(20);
        
        result[1].Name.Should().Be("$2");
        result[1].Value.Should().Be(true);
    }
}
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

    public PostgresQueryParameterBinderTests()
    {
        var formatterSubstitute = Substitute.For<IDatabaseSpecificSyntaxFormatter>();
        _sut = new PostgresQueryParameterBinder(formatterSubstitute);
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

        //act
        var result = _sut.BindParameters(query);

        //assert
        result.Should().HaveCount(2);
        result[0].Value.Should().Be(20);
        result[1].Value.Should().Be(true);
    }
}
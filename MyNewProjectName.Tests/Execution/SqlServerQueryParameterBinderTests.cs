using System;
using System.Collections.Generic;
using FluentAssertions;
using MyNewProjectName.Core;
using MyNewProjectName.Execution.Business;
using MyNewProjectName.Grammars.Abstractions;
using NSubstitute;
using Xunit;

namespace MyNewProjectName.Tests.Execution;

public class SqlServerQueryParameterBinderTests
{
    private readonly SqlServerQueryParameterBinder _sut;
    private readonly IDatabaseSpecificSyntaxFormatter _formatterSubstitute;

    public SqlServerQueryParameterBinderTests()
    {
        _formatterSubstitute = Substitute.For<IDatabaseSpecificSyntaxFormatter>();
        _sut = new SqlServerQueryParameterBinder(_formatterSubstitute);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenFormatterIsNull()
    {
        // Arrange
        // Act
        Action act = () => new SqlServerQueryParameterBinder(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("formatter");
    }

    [Fact]
    public void BindParameters_ShouldThrowArgumentNullException_WhenQueryIsNull()
    {
        // Arrange
        // Act
        Action act = () => _sut.BindParameters(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("query");
    }

    [Fact]
    public void BindParameters_ShouldReturnEmptyList_WhenWhereConditionsAreEmpty()
    {
        // Arrange
        var query = new Query();

        // Act
        var result = _sut.BindParameters(query);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void BindParameters_ShouldReturnQueryParameters_WhenWhereConditionsAreProvided()
    {
        // Arrange
        var query = new Query()
            .Where("age", 20)
            .Where("ismale", true);

        _formatterSubstitute.ParameterStartIndex.Returns(0);
        _formatterSubstitute.GetParameterName(0).Returns("@p0");
        _formatterSubstitute.GetParameterName(1).Returns("@p1");

        // Act
        var result = _sut.BindParameters(query);

        // Assert
        result.Should().HaveCount(2);
        result[0].Name.Should().Be("@p0");
        result[0].Value.Should().Be(20);
        result[1].Name.Should().Be("@p1");
        result[1].Value.Should().Be(true);
    }

    [Fact]
    public void BindParameters_ShouldSkipNullValuesAndNotIncrementIndex_WhenValueIsNull()
    {
        // Arrange
        var query = new Query()
            .Where("firstname", null!) 
            .Where("age", 20);         

        _formatterSubstitute.ParameterStartIndex.Returns(0);
        _formatterSubstitute.GetParameterName(0).Returns("@p0");

        // Act
        var result = _sut.BindParameters(query);

        // Assert
        result.Should().ContainSingle(); 
        
        result[0].Name.Should().Be("@p0"); 
        result[0].Value.Should().Be(20);
        
        _formatterSubstitute.Received(1).GetParameterName(0);
    }
}
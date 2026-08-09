using System;
using System.Collections.Generic;
using MyNewProjectName.Compilers.Business;
using MyNewProjectName.Core;
using MyNewProjectName.Grammars.Abstractions;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace MyNewProjectName.Tests.Compilers;

public class WhereConditionProcessorTests
{
    private readonly IDatabaseSpecificSyntaxFormatter _formatterSubstitute;
    private readonly WhereConditionProcessor _sut;

    public WhereConditionProcessorTests()
    {
        _formatterSubstitute = Substitute.For<IDatabaseSpecificSyntaxFormatter>();
        _sut = new WhereConditionProcessor(_formatterSubstitute);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenFormatterIsNull()
    {
        var act = () => new WhereConditionProcessor(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Process_ShouldReturnEmptyConditionsAndBindings_WhenClausesAreEmpty()
    {
        var result = _sut.Process(new List<WhereCondition>());

        result.Conditions.Should().BeEmpty();
        result.BindingValues.Should().BeEmpty();
    }

    [Fact]
    public void Process_ShouldUseFormatterToBuildCondition_WhenClauseIsValid()
    {
        // Arrange
        var clauses = new List<WhereCondition> { new("grade", 19.24m) };

        _formatterSubstitute.ParameterStartIndex.Returns(1);
        _formatterSubstitute.FormatIdentifier("grade").Returns("\"grade\"");
        _formatterSubstitute.GetParameterName(1).Returns("$1");

        // Act
        var result = _sut.Process(clauses);

        // Assert
        result.Conditions.Should().ContainSingle().Which.Should().Be("\"grade\" = $1");
        result.BindingValues.Should().ContainSingle().Which.Should().Be(19.24m);
        
        _formatterSubstitute.Received(1).FormatIdentifier("grade");
        _formatterSubstitute.Received(1).GetParameterName(1);
    }

    [Fact]
    public void Process_ShouldFormatBooleanCondition_WhenValueIsBoolean()
    {
        // Arrange
        var clauses = new List<WhereCondition> { new("ismale", false) };

        _formatterSubstitute.ParameterStartIndex.Returns(1);
        _formatterSubstitute.FormatIdentifier("ismale").Returns("\"ismale\"");
        _formatterSubstitute.GetParameterName(1).Returns("$1");

        // Act
        var result = _sut.Process(clauses);

        // Assert
        result.Conditions.Should().ContainSingle().Which.Should().Be("\"ismale\" = $1");
        result.BindingValues.Should().ContainSingle().Which.Should().Be(false);
    }

    [Fact]
    public void Process_ShouldFormatStringCondition_WhenValueIsString()
    {
        // Arrange
        var clauses = new List<WhereCondition> { new("firstname", "Ali") };

        _formatterSubstitute.ParameterStartIndex.Returns(1);
        _formatterSubstitute.FormatIdentifier("firstname").Returns("\"firstname\"");
        _formatterSubstitute.GetParameterName(1).Returns("$1");

        // Act
        var result = _sut.Process(clauses);

        // Assert
        result.Conditions.Should().ContainSingle().Which.Should().Be("\"firstname\" = $1");
        result.BindingValues.Should().ContainSingle().Which.Should().Be("Ali");
    }

    [Fact]
    public void Process_ShouldReplaceWithDBNullValue_WhenValueIsNull()
    {
        // Arrange
        var clauses = new List<WhereCondition> { new("firstname", null!) };

        _formatterSubstitute.ParameterStartIndex.Returns(1);
        _formatterSubstitute.FormatIdentifier("firstname").Returns("\"firstname\"");
        _formatterSubstitute.GetParameterName(1).Returns("$1");

        // Act
        var result = _sut.Process(clauses);

        // Assert
        result.Conditions.Should().ContainSingle().Which.Should().Be("\"firstname\" = $1");
        result.BindingValues.Should().ContainSingle().Which.Should().Be(DBNull.Value);
    }

    [Fact]
    public void Process_ShouldFormatMultipleConditionsInOrder_WhenMultipleClausesProvided()
    {
        // Arrange
        var clauses = new List<WhereCondition>
        {
            new("ismale", false),
            new("grade", 19.24m),
            new("age", 20)
        };

        _formatterSubstitute.ParameterStartIndex.Returns(1);
        _formatterSubstitute.FormatIdentifier("ismale").Returns("\"ismale\"");
        _formatterSubstitute.FormatIdentifier("grade").Returns("\"grade\"");
        _formatterSubstitute.FormatIdentifier("age").Returns("\"age\"");

        _formatterSubstitute.GetParameterName(1).Returns("$1");
        _formatterSubstitute.GetParameterName(2).Returns("$2");
        _formatterSubstitute.GetParameterName(3).Returns("$3");

        // Act
        var result = _sut.Process(clauses);

        // Assert
        var expectedConditions = new[] { "\"ismale\" = $1", "\"grade\" = $2", "\"age\" = $3" };
        var expectedBindings = new object[] { false, 19.24m, 20 };

        result.Conditions.Should().Equal(expectedConditions);
        result.BindingValues.Should().Equal(expectedBindings);
    }
    
}

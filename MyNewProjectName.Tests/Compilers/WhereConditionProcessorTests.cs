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
    private readonly WhereConditionProcessor sut;

    public WhereConditionProcessorTests()
    {
        _formatterSubstitute = Substitute.For<IDatabaseSpecificSyntaxFormatter>();
        sut = new WhereConditionProcessor(_formatterSubstitute);
    }

    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_FormatterIsNull()
    {
        Action act = () => new WhereConditionProcessor(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Process_Should_ReturnEmptyConditionsAndBindings_When_ClausesAreEmpty()
    {
        var result = sut.Process(new List<WhereCondition>());

        result.Conditions.Should().BeEmpty();
        result.BindingValues.Should().BeEmpty();
    }

    [Fact]
    public void Process_Should_UseFormatterToBuildCondition_When_ClauseIsValid()
    {
        // Arrange
        var clauses = new List<WhereCondition> { new("grade", 19.24m) };

        _formatterSubstitute.ParameterStartIndex.Returns(1);
        _formatterSubstitute.FormatIdentifier("grade").Returns("\"grade\"");
        _formatterSubstitute.GetParameterName(1).Returns("$1");

        // Act
        var result = sut.Process(clauses);

        // Assert
        result.Conditions.Should().ContainSingle().Which.Should().Be("\"grade\" = $1");
        result.BindingValues.Should().ContainSingle().Which.Should().Be(19.24m);

        // Verify
        _formatterSubstitute.Received(1).FormatIdentifier("grade");
        _formatterSubstitute.Received(1).GetParameterName(1);
    }

    [Fact]
    public void Process_Should_ReplaceNullWithDBNullValue_When_ValueIsNull()
    {
        // Arrange
        var clauses = new List<WhereCondition> { new("firstname", null!) };

        _formatterSubstitute.ParameterStartIndex.Returns(1);
        _formatterSubstitute.FormatIdentifier("firstname").Returns("\"firstname\"");
        _formatterSubstitute.GetParameterName(1).Returns("$1");

        // Act
        var result = sut.Process(clauses);

        // Assert
        result.BindingValues.Should().ContainSingle().Which.Should().Be(DBNull.Value);
    }
}
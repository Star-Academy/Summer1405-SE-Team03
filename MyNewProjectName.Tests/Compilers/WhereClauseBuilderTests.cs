using System;
using System.Collections.Generic;
using FluentAssertions;
using MyNewProjectName.Compilers.Abstractions;
using MyNewProjectName.Compilers.Business;
using MyNewProjectName.Core;
using NSubstitute;
using Xunit;

namespace MyNewProjectName.Tests.Compilers;

public class WhereClauseBuilderTests
{
    private readonly IWhereConditionProcessor _whereConditionProcessorSubstitute;
    private readonly WhereClauseBuilder _sut;

    public WhereClauseBuilderTests()
    {
        _whereConditionProcessorSubstitute = Substitute.For<IWhereConditionProcessor>();
        _sut = new WhereClauseBuilder(_whereConditionProcessorSubstitute);
    }

    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_ProcessorIsNull()
    {
        // Arrange & Act
        var action = () => new WhereClauseBuilder(null!);

        // Assert
        action.Should().Throw<ArgumentNullException>()
              .WithParameterName("whereConditionProcessor");
    }

    [Fact]
    public void Build_Should_ReturnEmptyResult_When_QueryHasNoWhereConditions()
    {
        // Arrange
        var query = new Query();

        // Act
        var result = _sut.Build(query);

        // Assert
        result.SqlText.Should().BeEmpty();
        result.BindingValues.Should().BeEmpty();
    }

    [Fact]
    public void Build_Should_ReturnFormattedClause_When_ConditionValueIsBoolean()
    {
        // Arrange
        var query = new Query().Where("ismale", false);

        var processedResult = new ProcessedWhereConditions(
            Conditions: new List<string> { "\"ismale\" = $1" },
            BindingValues: new List<object> { false }
        );

        _whereConditionProcessorSubstitute.Process(query.WhereConditions).Returns(processedResult);

        // Act
        var result = _sut.Build(query);

        // Assert
        result.SqlText.Should().Be(" WHERE \"ismale\" = $1");
        result.BindingValues.Should().ContainSingle().Which.Should().Be(false);
    }

    [Fact]
    public void Build_Should_ReturnFormattedClause_When_ConditionValueIsDecimal()
    {
        // Arrange
        var query = new Query().Where("grade", 19.24m);

        var processedResult = new ProcessedWhereConditions(
            Conditions: new List<string> { "\"grade\" = $1" },
            BindingValues: new List<object> { 19.24m }
        );

        _whereConditionProcessorSubstitute.Process(query.WhereConditions).Returns(processedResult);

        // Act
        var result = _sut.Build(query);

        // Assert
        result.SqlText.Should().Be(" WHERE \"grade\" = $1");
        result.BindingValues.Should().ContainSingle().Which.Should().Be(19.24m);
    }

    [Fact]
    public void Build_Should_ReturnFormattedClause_When_ConditionValueIsString()
    {
        // Arrange
        var query = new Query().Where("firstname", "Ali");

        var processedResult = new ProcessedWhereConditions(
            Conditions: new List<string> { "\"firstname\" = $1" },
            BindingValues: new List<object> { "Ali" }
        );

        _whereConditionProcessorSubstitute.Process(query.WhereConditions).Returns(processedResult);

        // Act
        var result = _sut.Build(query);

        // Assert
        result.SqlText.Should().Be(" WHERE \"firstname\" = $1");
        result.BindingValues.Should().ContainSingle().Which.Should().Be("Ali");
    }

    [Fact]
    public void Build_Should_ReturnFormattedClause_When_ConditionValueIsNull()
    {
        // Arrange
        var query = new Query().Where("firstname", null!);

        var processedResult = new ProcessedWhereConditions(
            Conditions: new List<string> { "\"firstname\" = $1" },
            BindingValues: new List<object> { DBNull.Value }
        );

        _whereConditionProcessorSubstitute.Process(query.WhereConditions).Returns(processedResult);

        // Act
        var result = _sut.Build(query);

        // Assert
        result.SqlText.Should().Be(" WHERE \"firstname\" = $1");
        result.BindingValues.Should().ContainSingle().Which.Should().Be(DBNull.Value);
    }

    [Fact]
    public void Build_Should_ReturnAndJoinedSql_When_QueryHasMultipleWhereConditions()
    {
        // Arrange
        var query = new Query()
            .Where("ismale", false)
            .Where("grade", 19.24m)
            .Where("age", 20);

        var processedResult = new ProcessedWhereConditions(
            Conditions: new List<string> { "\"ismale\" = $1", "\"grade\" = $2", "\"age\" = $3" },
            BindingValues: new List<object> { false, 19.24m, 20 }
        );

        _whereConditionProcessorSubstitute.Process(query.WhereConditions).Returns(processedResult);

        // Act
        var result = _sut.Build(query);

        // Assert
        result.SqlText.Should().Be(" WHERE \"ismale\" = $1 AND \"grade\" = $2 AND \"age\" = $3");
        result.BindingValues.Should().Equal(processedResult.BindingValues);

        _whereConditionProcessorSubstitute.Received(1).Process(query.WhereConditions);
    }
}
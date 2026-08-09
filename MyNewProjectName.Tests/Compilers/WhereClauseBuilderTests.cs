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
    public void Constructor_ShouldThrowArgumentNullException_WhenProcessorIsNull()
    {
        // Arrange & Act
        var action = () => new WhereClauseBuilder(null!);

        // Assert
        action.Should().Throw<ArgumentNullException>()
              .WithParameterName("whereConditionProcessor");
    }

    [Fact]
    public void Build_ShouldReturnEmptyResult_WhenQueryHasNoWhereConditions()
    {
        // Arrange
        var query = new Query();

        // Act
        var result = _sut.Build(query);

        // Assert
        result.SqlText.Should().BeEmpty();
    }

    [Fact]
    public void Build_ShouldReturnFormattedClause_WhenConditionValueIsBoolean()
    {
        // Arrange
        var query = new Query().Where("ismale", false);

        var processedResult = new ProcessedWhereConditions(
            new List<string> { "\"ismale\" = $1" }
        );

        _whereConditionProcessorSubstitute.Process(query.WhereConditions).Returns(processedResult);

        // Act
        var result = _sut.Build(query);

        // Assert
        result.SqlText.Should().Be(" WHERE \"ismale\" = $1");
    }

    [Fact]
    public void Build_ShouldReturnFormattedClause_WhenConditionValueIsDecimal()
    {
        // Arrange
        var query = new Query().Where("grade", 19.24m);

        var processedResult = new ProcessedWhereConditions(
            new List<string> { "\"grade\" = $1" }
        );

        _whereConditionProcessorSubstitute.Process(query.WhereConditions).Returns(processedResult);

        // Act
        var result = _sut.Build(query);

        // Assert
        result.SqlText.Should().Be(" WHERE \"grade\" = $1");
    }

    [Fact]
    public void Build_ShouldReturnFormattedClause_WhenConditionValueIsString()
    {
        // Arrange
        var query = new Query().Where("firstname", "Ali");

        var processedResult = new ProcessedWhereConditions(
            new List<string> { "\"firstname\" = $1" }
        );

        _whereConditionProcessorSubstitute.Process(query.WhereConditions).Returns(processedResult);

        // Act
        var result = _sut.Build(query);

        // Assert
        result.SqlText.Should().Be(" WHERE \"firstname\" = $1");
    }

    [Fact]
    public void Build_ShouldReturnFormattedClause_WhenConditionValueIsNull()
    {
        // Arrange
        var query = new Query().Where("firstname", null!);

        var processedResult = new ProcessedWhereConditions(
            new List<string> { "\"firstname\" = $1" }
        );

        _whereConditionProcessorSubstitute.Process(query.WhereConditions).Returns(processedResult);

        // Act
        var result = _sut.Build(query);

        // Assert
        result.SqlText.Should().Be(" WHERE \"firstname\" = $1");
    }

    [Fact]
    public void Build_ShouldReturnAndJoinedSql_WhenQueryHasMultipleWhereConditions()
    {
        // Arrange
        var query = new Query()
            .Where("ismale", false)
            .Where("grade", 19.24m)
            .Where("age", 20);

        var processedResult = new ProcessedWhereConditions(
            new List<string> { "\"ismale\" = $1", "\"grade\" = $2", "\"age\" = $3" }
        );

        _whereConditionProcessorSubstitute.Process(query.WhereConditions).Returns(processedResult);

        // Act
        var result = _sut.Build(query);

        // Assert
        result.SqlText.Should().Be(" WHERE \"ismale\" = $1 AND \"grade\" = $2 AND \"age\" = $3");
        _whereConditionProcessorSubstitute.Received(1).Process(query.WhereConditions);
    }
}
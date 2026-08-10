using System;
using System.Collections.Generic;
using FluentAssertions;
using MyNewProjectName.Compilers.Abstractions;
using MyNewProjectName.Compilers.Business;
using MyNewProjectName.Core;
using NSubstitute;
using Xunit;

namespace MyNewProjectName.Tests.Compilers;

public class SqlQueryCompilerTests
{
    private readonly ISelectClauseBuilder _selectClauseBuilderSubstitute;
    private readonly IFromClauseBuilder _fromClauseBuilderSubstitute;
    private readonly IWhereClauseBuilder _whereClauseBuilderSubstitute;
    private readonly SqlQueryCompiler _sut;

    public SqlQueryCompilerTests()
    {
        _selectClauseBuilderSubstitute = Substitute.For<ISelectClauseBuilder>();
        _fromClauseBuilderSubstitute = Substitute.For<IFromClauseBuilder>();
        _whereClauseBuilderSubstitute = Substitute.For<IWhereClauseBuilder>();

        _sut = new SqlQueryCompiler(
            _selectClauseBuilderSubstitute,
            _fromClauseBuilderSubstitute,
            _whereClauseBuilderSubstitute);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenSelectClauseBuilderIsNull()
    {
        // Arrange
        // Act
        var action = () => new SqlQueryCompiler(null!, _fromClauseBuilderSubstitute, _whereClauseBuilderSubstitute);

        // Assert
        action.Should().Throw<ArgumentNullException>()
            .WithParameterName("selectClauseBuilder");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenFromClauseBuilderIsNull()
    {
        // Arrange
        // Act
        var action = () => new SqlQueryCompiler(_selectClauseBuilderSubstitute, null!, _whereClauseBuilderSubstitute);

        // Assert
        action.Should().Throw<ArgumentNullException>()
            .WithParameterName("fromClauseBuilder");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenWhereClauseBuilderIsNull()
    {
        // Arrange
        // Act
        var action = () => new SqlQueryCompiler(_selectClauseBuilderSubstitute, _fromClauseBuilderSubstitute, null!);

        // Assert
        action.Should().Throw<ArgumentNullException>()
            .WithParameterName("whereClauseBuilder");
    }

    [Fact]
    public void Compile_ShouldBuildSqlWithoutWhereClause_WhenWhereClauseResultIsEmpty()
    {
        // Arrange
        var query = new Query().From("student");

        _selectClauseBuilderSubstitute.Build(query).Returns("SELECT *");
        _fromClauseBuilderSubstitute.Build(query).Returns(" FROM \"student\"");
        _whereClauseBuilderSubstitute.Build(query).Returns(new WhereClauseResult(string.Empty));

        // Act
        var result = _sut.Compile(query);

        // Assert
        result.SqlQuery.Should().Be("SELECT * FROM \"student\"");

        Received.InOrder(() =>
        {
            _selectClauseBuilderSubstitute.Build(query);
            _fromClauseBuilderSubstitute.Build(query);
            _whereClauseBuilderSubstitute.Build(query);
        });
    }

    [Fact]
    public void Compile_ShouldBuildSqlWithSingleWhereClause_WhenQueryHasOneCondition()
    {
        // Arrange
        var query = new Query()
            .From("student")
            .Select("firstname")
            .Where("ismale", false);

        _selectClauseBuilderSubstitute.Build(query).Returns("SELECT \"firstname\"");
        _fromClauseBuilderSubstitute.Build(query).Returns(" FROM \"student\"");
        _whereClauseBuilderSubstitute.Build(query).Returns(new WhereClauseResult(" WHERE \"ismale\" = $1"));

        // Act
        var result = _sut.Compile(query);

        // Assert
        result.SqlQuery.Should().Be("SELECT \"firstname\" FROM \"student\" WHERE \"ismale\" = $1");

        Received.InOrder(() =>
        {
            _selectClauseBuilderSubstitute.Build(query);
            _fromClauseBuilderSubstitute.Build(query);
            _whereClauseBuilderSubstitute.Build(query);
        });
    }

    [Fact]
    public void Compile_ShouldBuildSqlWithMultipleWhereClauses_WhenQueryHasMultipleConditions()
    {
        // Arrange
        var query = new Query()
            .From("student")
            .Select("studentnumber", "firstname")
            .Where("ismale", false)
            .Where("grade", 19.24m);

        _selectClauseBuilderSubstitute.Build(query).Returns("SELECT \"studentnumber\", \"firstname\"");
        _fromClauseBuilderSubstitute.Build(query).Returns(" FROM \"student\"");
        _whereClauseBuilderSubstitute.Build(query).Returns(new WhereClauseResult(" WHERE \"ismale\" = $1 AND \"grade\" = $2"));

        // Act
        var result = _sut.Compile(query);

        // Assert
        result.SqlQuery.Should().Be("SELECT \"studentnumber\", \"firstname\" FROM \"student\" WHERE \"ismale\" = $1 AND \"grade\" = $2");
    }

    [Fact]
    public void Compile_ShouldPassThroughDBNullBinding_WhenWhereConditionValueIsNull()
    {
        // Arrange
        var query = new Query()
            .From("student")
            .Where("firstname", null!);

        _selectClauseBuilderSubstitute.Build(query).Returns("SELECT *");
        _fromClauseBuilderSubstitute.Build(query).Returns(" FROM \"student\"");
        _whereClauseBuilderSubstitute.Build(query).Returns(new WhereClauseResult(" WHERE \"firstname\" = $1"));

        // Act
        var result = _sut.Compile(query);

        // Assert
        result.SqlQuery.Should().Be("SELECT * FROM \"student\" WHERE \"firstname\" = $1");
    }
}
using System;
using System.Collections.Generic;
using FluentAssertions;
using MyNewProjectName.Compilers.Business;
using MyNewProjectName.Core;
using MyNewProjectName.Execution.Business;
using MyNewProjectName.Grammars.Business;
using MyNewProjectName.IntegrationTests.Fixtures;
using MyNewProjectName.Presentation.Abstractions;
using NSubstitute;
using Xunit;

namespace MyNewProjectName.IntegrationTests.Execution;

public class PostgresExecutionIntegrationTests : IClassFixture<PostgresDatabaseFixture>
{
    private readonly QueryExecutionOrchestrator _sut;

    public PostgresExecutionIntegrationTests(PostgresDatabaseFixture postgresDatabaseFixture)
    {
        var formatter = new PostgresSyntaxFormatter();
        var compiler = new SqlQueryCompiler(
            new SelectClauseBuilder(formatter),
            new FromClauseBuilder(formatter),
            new WhereClauseBuilder(new WhereConditionProcessor(formatter))
        );

        var runner = new DatabaseQueryRunner(
            new PostgresConnectionFactory(postgresDatabaseFixture.ConnectionString),
            new PostgresCommandFactory(),
            new PostgresQueryParameterBinder(formatter),
            Substitute.For<IQueryResultPresenter>()
        );

        _sut = new QueryExecutionOrchestrator(compiler, runner);
    }

    [Fact]
    public void ExecuteQuery_ShouldReturnAllInsertedRows_WhenNoWhereConditionIsProvided()
    {
        // arrange
        var query = new Query().From("student").Select("firstname");

        // act
        using var reader = _sut.ExecuteQuery(query);

        // assert
        var count = 0;
        while (reader.Read()) count++;

        count.Should().Be(6);
    }

    [Fact]
    public void ExecuteQuery_ShouldReturnOnlySelectedColumns_WhenSpecificColumnsAreProvided()
    {
        // arrange
        var query = new Query().From("student").Select("firstname", "ismale");

        // act
        using var reader = _sut.ExecuteQuery(query);

        // assert
        var count = 0;
        var columnNames = new List<string>();
        for (var i = 0; i < reader.FieldCount; i++)
        {
            columnNames.Add(reader.GetName(i));
        }

        while (reader.Read()) count++;

        count.Should().Be(6);
        columnNames.Count.Should().Be(2);
        columnNames.Should().Contain(new[] { "firstname", "ismale" });
    }

    [Fact]
    public void ExecuteQuery_ShouldReturnCorrectRows_WhenWhereConditionIsBool()
    {
        // arrange
        var query = new Query().From("student").Select("firstname").Where("ismale", true);

        // act
        using var reader = _sut.ExecuteQuery(query);

        // assert
        var count = 0;
        while (reader.Read()) count++;

        count.Should().Be(3);
    }

    [Fact]
    public void ExecuteQuery_ShouldReturnCorrectRows_WhenWhereConditionIsDecimal()
    {
        // arrange
        var query = new Query().From("student").Select("firstname").Where("grade", 19.24m);

        // act
        using var reader = _sut.ExecuteQuery(query);

        // assert
        var count = 0;
        while (reader.Read()) count++;

        count.Should().Be(3);
    }

    [Fact]
    public void ExecuteQuery_ShouldReturnCorrectData_WhenAllTheWhereConditionsProvided()
    {
        // arrange
        var query = new Query()
            .From("student")
            .Select("firstname")
            .Where("ismale", true)
            .Where("grade", 19.24m);

        // act
        using var reader = _sut.ExecuteQuery(query);

        // assert
        var count = 0;
        string? returnName = null;
        while (reader.Read())
        {
            count++;
            returnName = reader["firstname"].ToString();
        }

        count.Should().Be(1);
        returnName.Should().Be("Mahdi");
    }

    [Fact]
    public void ExecuteQuery_ShouldReturnZeroRows_WhenNoMatchingDataExists()
    {
        // arrange
        var query = new Query().From("student").Select("firstname").Where("grade", 100m);

        // act
        using var reader = _sut.ExecuteQuery(query);

        // assert
        var count = 0;
        while (reader.Read()) count++;

        count.Should().Be(0);
    }

    [Fact]
    public void ExecuteQuery_ShouldExecuteWithoutError_WhenWhereConditionIsEqualToNull()
    {
        // arrange
        var query = new Query().From("student").Select("studentnumber").Where("firstname", null);

        // act
        var act = () => _sut.ExecuteQuery(query);

        // assert
        act.Should().NotThrow();
        using var reader = act();
        var count = 0;
        while (reader.Read()) count++;
        
        count.Should().Be(1);
    }

    [Fact]
    public void ExecuteQuery_ShouldThrowInvalidOperationException_WhenTableDoesNotExist()
    {
        // arrange
        var query = new Query().From("table_that_does_not_exist").Select("some_column");

        // act
        var act = () => _sut.ExecuteQuery(query);

        // assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void ExecuteQuery_ShouldReturnAllColumns_WhenNoSelectClauseIsProvided()
    {
        // arrange
        var query = new Query().From("student");

        // act
        using var reader = _sut.ExecuteQuery(query);

        // assert
        reader.FieldCount.Should().Be(4);
    }

    [Fact]
    public void ExecuteQuery_ShouldReturnCorrectRows_WhenWhereConditionIsString()
    {
        // arrange
        var query = new Query().From("student").Select("studentnumber").Where("firstname", "Amir");

        // act
        using var reader = _sut.ExecuteQuery(query);

        // assert
        var count = 0;
        while (reader.Read()) count++;

        count.Should().Be(1);
    }

    [Fact]
    public void ExecuteQuery_ShouldHandleSpacesInTableAndColumnNames_WhenFormatIdentifierIsUsed()
    {
        // arrange
        var query = new Query().From("my students").Select("first name").Where("first name", "Amir");

        // act
        using var reader = _sut.ExecuteQuery(query);

        // assert
        var count = 0;
        while (reader.Read()) count++;

        count.Should().Be(1);
    }

    [Fact]
    public void ExecuteQuery_ShouldReturnDBNullForNullColumn_WhenDatabaseContainsNullValue()
    {
        // arrange
        var query = new Query().From("student").Select("firstname").Where("studentnumber", 6);

        // act
        using var reader = _sut.ExecuteQuery(query);

        // assert
        var count = 0;
        while (reader.Read())
        {
            count++;
            reader.IsDBNull(reader.GetOrdinal("firstname")).Should().BeTrue();
        }
        count.Should().Be(1);
    }

    [Fact]
    public void ExecuteQuery_ShouldReturnZeroRows_WhenWhereValueIsLiteralStringNull()
    {
        // arrange
        var query = new Query().From("student").Select("firstname").Where("firstname", "null");

        // act
        using var reader = _sut.ExecuteQuery(query);

        // assert
        var count = 0;
        while (reader.Read()) count++;
        
        count.Should().Be(0);
    }

    [Fact]
    public void ExecuteQuery_ShouldReturnZeroRows_WhenWhereValueContainsSpecialCharacters()
    {
        // arrange
        var query = new Query()
            .From("student")
            .Select("firstname")
            .Where("firstname", "' OR '1'='1");

        // act
        using var reader = _sut.ExecuteQuery(query);

        // assert
        var count = 0;
        while (reader.Read()) count++;
        
        count.Should().Be(0);
    }

    [Fact]
    public void ExecuteQuery_ShouldReturnMatchingRows_WhenWhereConditionChecksForDatabaseNull()
    {
        // arrange
        var query = new Query().From("student").Select("firstname", "grade").Where("firstname", null);

        // act
        using var reader = _sut.ExecuteQuery(query);

        // assert
        var count = 0;
        while (reader.Read())
        {
            count++;
            reader.IsDBNull(reader.GetOrdinal("firstname")).Should().BeTrue();
        }
        
        count.Should().BeGreaterThan(0);
    }
}
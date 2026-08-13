using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

public class SqlServerExecutionIntegrationTests : IClassFixture<SqlServerDatabaseFixture>, IAsyncLifetime
{
    private QueryExecutionOrchestrator _sut = null!;
    private readonly SqlServerDatabaseFixture _fixture;

    public SqlServerExecutionIntegrationTests(SqlServerDatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    public Task InitializeAsync()
    {
        var formatter = new SqlServerSyntaxFormatter();
        var compiler = new SqlQueryCompiler(
            new SelectClauseBuilder(formatter),
            new FromClauseBuilder(formatter),
            new WhereClauseBuilder(new WhereConditionProcessor(formatter))
        );

        var runner = new DatabaseQueryRunner(
            new SqlServerConnectionFactory(_fixture.ConnectionString),
            new SqlServerCommandFactory(),
            new SqlServerQueryParameterBinder(formatter),
            Substitute.For<IQueryResultPresenter>()
        );

        _sut = new QueryExecutionOrchestrator(compiler, runner);

        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
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
    public void ExecuteQuery_ShouldThrowInvalidOperationException_WhenTableDoesNotExist()
    {
        // arrange
        var query = new Query().From("NonExistentTable");

        // act
        var act = () => _sut.ExecuteQuery(query);

        // assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void ExecuteQuery_ShouldThrowInvalidOperationException_WhenSelectColumnDoesNotExist()
    {
        // arrange
        var query = new Query().From("student").Select("InvalidColumnName");

        // act
        var act = () => _sut.ExecuteQuery(query);

        // assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void ExecuteQuery_ShouldThrowInvalidOperationException_WhenWhereColumnDoesNotExist()
    {
        // arrange
        var query = new Query().From("student").Where("InvalidColumnName", "SomeValue");

        // act
        var act = () => _sut.ExecuteQuery(query);

        // assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void ExecuteQuery_ShouldReturnMatchingRows_WhenWhereValueIsNull()
    {
        // arrange
        var query = new Query().From("student").Where("firstname", null);

        // act
        using var reader = _sut.ExecuteQuery(query);

        // assert
        var count = 0;
        while (reader.Read()) count++;

        count.Should().BeGreaterThan(0);
    }

    [Fact]
    public void ExecuteQuery_ShouldReturnZeroRows_WhenFilteringByEmptyString()
    {
        // arrange
        var query = new Query().From("student").Where("firstname", "");

        // act
        using var reader = _sut.ExecuteQuery(query);

        // assert
        var count = 0;
        while (reader.Read()) count++;

        count.Should().Be(0);
    }

    [Fact]
    public void ExecuteQuery_ShouldHandleSpacesInTableName_WhenFormatIdentifierIsUsed()
    {
        // arrange
        var query = new Query().From("my students");

        // act
        using var reader = _sut.ExecuteQuery(query);

        // assert
        var count = 0;
        while (reader.Read()) count++;

        count.Should().BeGreaterThan(0);
    }

    [Fact]
    public void ExecuteQuery_ShouldHandleSpacesInColumnName_WhenFormatIdentifierIsUsed()
    {
        // arrange
        var query = new Query().From("my students").Select("student id");

        // act
        using var reader = _sut.ExecuteQuery(query);

        // assert
        var count = 0;
        var hasColumn = false;

        for (var i = 0; i < reader.FieldCount; i++)
        {
            if (reader.GetName(i) == "student id")
            {
                hasColumn = true;
            }
        }

        while (reader.Read()) count++;

        count.Should().BeGreaterThan(0);
        hasColumn.Should().BeTrue();
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
using System.Data;
using System.Threading.Tasks;
using FluentAssertions;
using MyNewProjectName.Compilers.Business;
using MyNewProjectName.Core;
using MyNewProjectName.Execution.Business;
using MyNewProjectName.Grammars.Business;
using MyNewProjectName.IntegrationTests.Fixtures;
using MyNewProjectName.Presentation.Abstractions;
using Npgsql;
using NSubstitute;
using Xunit;
namespace MyNewProjectName.IntegrationTests.Execution;

public class PostgresExecutionIntegrationTests : IClassFixture<PostgresDatabaseFixture>
{
    private readonly PostgresDatabaseFixture _fixture;
    
    public PostgresExecutionIntegrationTests(PostgresDatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task CanConnectToDatabase_ShouldReturnOne_WhenSimpleQueryIsExecuted()
    {
        //arrange
        var connectionString = _fixture.ConnectionString;
        await using var connection = new NpgsqlConnection(connectionString);
        //act
        await connection.OpenAsync();
        await using var command = connection.CreateCommand();
        command.CommandText = "SELECT 1";
        var result = await command.ExecuteScalarAsync();
        //assert
        result.Should().NotBeNull();
        long.Parse(result.ToString() ?? "").Should().Be(1);
    }

    [Fact]
    public async Task ExecuteQuery_ShouldReturnCorrectData_WhenAllTheWhereConditionsProvided()
    {
        //arrange
        var connectionString = _fixture.ConnectionString;
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        await using var createCommand = connection.CreateCommand();
        createCommand.CommandText = @"
            CREATE TABLE student(
            studentnumber INT PRIMARY KEY,
            firstname VARCHAR(100),
            ismale BOOLEAN,
            grade DECIMAL
            );
            INSERT INTO student(studentnumber, firstname, ismale ,  grade) VALUES (1, 'Amir', true, 18.24), (2 , 'Mahdi', true, 19.24) , (3 , 'Zahra', false, 19.24)";
        await createCommand.ExecuteNonQueryAsync();
        var postgresSyntaxFormatter = new PostgresSyntaxFormatter();
        var sqlQueryCompiler = new SqlQueryCompiler(new SelectClauseBuilder(postgresSyntaxFormatter), new FromClauseBuilder(postgresSyntaxFormatter), new WhereClauseBuilder(new WhereConditionProcessor(postgresSyntaxFormatter)));
        var presenterMock = Substitute.For<IQueryResultPresenter>();
        var countOfMatchedRows = 0;
        string? returnName = null;
        presenterMock.When(x => x.PresentResults(Arg.Any<System.Data.IDataReader>())).Do(callInfo =>
        {
            var systemDataReader = callInfo.ArgAt<IDataReader>(0);
            while (systemDataReader.Read())
            {
                countOfMatchedRows++;
                returnName = systemDataReader["firstname"].ToString();
            }
        });
        var databaseQueryRunner = new DatabaseQueryRunner(new PostgresConnectionFactory(connectionString) , new PostgresCommandFactory(), new PostgresQueryParameterBinder(postgresSyntaxFormatter), presenterMock);
        var queryExecutionOrchestrator = new QueryExecutionOrchestrator(sqlQueryCompiler, databaseQueryRunner);
        var query = new Query()
            .From("student")
            .Select("firstname")
            .Where("ismale", true)
            .Where("grade", 19.24m);
        //act
        queryExecutionOrchestrator.ExecuteQuery(query);
        //assert
        countOfMatchedRows.Should().Be(1);
        returnName.Should().Be("Mahdi");
    }
    
}
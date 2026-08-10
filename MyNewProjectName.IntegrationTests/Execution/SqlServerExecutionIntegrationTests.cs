using System.Data;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using MyNewProjectName.Compilers.Business;
using MyNewProjectName.Core;
using MyNewProjectName.Execution.Business;
using MyNewProjectName.Grammars.Business;
using MyNewProjectName.IntegrationTests.Fixtures;
using MyNewProjectName.Presentation.Abstractions;
using NSubstitute;
using Xunit;

namespace MyNewProjectName.IntegrationTests.Execution;

public class SqlServerExecutionIntegrationTests : IClassFixture<SqlServerDatabaseFixture>
{
    private readonly SqlServerDatabaseFixture _fixture;

    public SqlServerExecutionIntegrationTests(SqlServerDatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task CanConnectToDatabase_ShouldReturnOne_WhenSimpleQueryIsExecuted()
    {
        //arrange
        var connectionString = _fixture.ConnectionString;
        await using var connection = new SqlConnection(connectionString);

        //act
        await connection.OpenAsync();
        await using var command = connection.CreateCommand();
        command.CommandText = "SELECT 1";
        var result = await command.ExecuteScalarAsync();

        //assert
        result.Should().NotBeNull();
        int.Parse(result.ToString() ?? "").Should().Be(1);
    }

    [Fact]
    public async Task ExecuteQuery_ShouldReturnAllInsertedRows_WhenNoWhereConditionIsProvided()
    {
        //arrange
        var connectionString = _fixture.ConnectionString;
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        await using var createCommand = connection.CreateCommand();
        
        createCommand.CommandText = @"
            DROP TABLE IF EXISTS student;
            CREATE TABLE student(
                studentnumber INT PRIMARY KEY,
                firstname VARCHAR(100),
                ismale BIT,
                grade DECIMAL(18, 2)
            );
            INSERT INTO student(studentnumber, firstname, ismale, grade) VALUES 
            (1, 'Amir', 'true', 18.24), 
            (2, 'Mahdi', 'true', 19.24), 
            (3, 'Zahra', 'false', 19.24)";

        await createCommand.ExecuteNonQueryAsync();
        
        var sqlServerSyntaxFormatter = new SqlServerSyntaxFormatter();
        var sqlQueryCompiler = new SqlQueryCompiler(
            new SelectClauseBuilder(sqlServerSyntaxFormatter),
            new FromClauseBuilder(sqlServerSyntaxFormatter),
            new WhereClauseBuilder(new WhereConditionProcessor(sqlServerSyntaxFormatter))
        );

        var presenterMock = Substitute.For<IQueryResultPresenter>();
        var countOfMatchedRows = 0;

        presenterMock.When(x => x.PresentResults(Arg.Any<System.Data.IDataReader>())).Do(callInfo =>
        {
            var systemDataReader = callInfo.ArgAt<System.Data.IDataReader>(0);
            while (systemDataReader.Read())
            {
                countOfMatchedRows++;
            }
        });
        
        var databaseQueryRunner = new DatabaseQueryRunner(
            new SqlServerConnectionFactory(connectionString),
            new SqlServerCommandFactory(), 
            new SqlServerQueryParameterBinder(sqlServerSyntaxFormatter), 
            presenterMock);
            
        var queryExecutionOrchestrator = new QueryExecutionOrchestrator(sqlQueryCompiler, databaseQueryRunner);
        
        var query = new Query()
            .From("student")
            .Select("firstname");

        //act
        queryExecutionOrchestrator.ExecuteQuery(query);

        //assert
        countOfMatchedRows.Should().Be(3);
    }

    [Fact]
    public async Task ExecuteQuery_ShouldReturnOnlySelectedColumns_WhenSpecificColumnsAreProvided()
    {
        //arrange
        var connectionString = _fixture.ConnectionString;
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        await using var createCommand = connection.CreateCommand();
        createCommand.CommandText = @"
            DROP TABLE IF EXISTS student;
            CREATE TABLE student(
                studentnumber INT PRIMARY KEY,
                firstname VARCHAR(100),
                ismale BIT,
                grade DECIMAL(18, 2)
            );
            INSERT INTO student(studentnumber, firstname, ismale, grade) VALUES (1, 'Amir', 'true', 18.24), (2, 'Mahdi', 'true', 19.24), (3, 'Zahra', 'false', 19.24), (4, 'sama', 'false', 19.24), (5, 'mohammad', 'true', 19.22)";

        await createCommand.ExecuteNonQueryAsync();

        var sqlServerSyntaxFormatter = new SqlServerSyntaxFormatter();
        var sqlQueryCompiler = new SqlQueryCompiler(new SelectClauseBuilder(sqlServerSyntaxFormatter),
            new FromClauseBuilder(sqlServerSyntaxFormatter),
            new WhereClauseBuilder(new WhereConditionProcessor(sqlServerSyntaxFormatter)));
        var presenterMock = Substitute.For<IQueryResultPresenter>();

        var countOfMatchedRows = 0;
        var columnNames = new List<string>();
        presenterMock.When(x => x.PresentResults(Arg.Any<System.Data.IDataReader>())).Do(callInfo =>
        {
            var systemDataReader = callInfo.ArgAt<IDataReader>(0);
            for (int i = 0; i < systemDataReader.FieldCount; i++)
            {
                columnNames.Add(systemDataReader.GetName(i));
            }

            while (systemDataReader.Read())
            {
                countOfMatchedRows++;
            }
        });

        var databaseQueryRunner = new DatabaseQueryRunner(new SqlServerConnectionFactory(connectionString),
            new SqlServerCommandFactory(), new SqlServerQueryParameterBinder(sqlServerSyntaxFormatter), presenterMock);
        var queryExecutionOrchestrator = new QueryExecutionOrchestrator(sqlQueryCompiler, databaseQueryRunner);
        var query = new Query()
            .From("student")
            .Select("firstname", "ismale");

        //act
        queryExecutionOrchestrator.ExecuteQuery(query);

        //assert
        countOfMatchedRows.Should().Be(5);
        columnNames.Count.Should().Be(2);
        columnNames.Should().Contain("firstname");
        columnNames.Should().Contain("ismale");
    }

    [Fact]
    public async Task ExecuteQuery_ShouldReturnCorrectRows_WhenBoolConditionIsProvided()
    {
        //arrange
        var connectionString = _fixture.ConnectionString;
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        await using var createCommand = connection.CreateCommand();
        createCommand.CommandText = @"
            DROP TABLE IF EXISTS student;
            CREATE TABLE student(
                studentnumber INT PRIMARY KEY,
                firstname VARCHAR(100),
                ismale BIT,
                grade DECIMAL(18, 2)
            );
            INSERT INTO student(studentnumber, firstname, ismale, grade) VALUES (1, 'Amir', 'true', 18.24), (2, 'Mahdi', 'true', 19.24), (3, 'Zahra', 'false', 19.24)";

        await createCommand.ExecuteNonQueryAsync();

        var sqlServerSyntaxFormatter = new SqlServerSyntaxFormatter();
        var sqlQueryCompiler = new SqlQueryCompiler(new SelectClauseBuilder(sqlServerSyntaxFormatter), new FromClauseBuilder(sqlServerSyntaxFormatter), new WhereClauseBuilder(new WhereConditionProcessor(sqlServerSyntaxFormatter)));
        var presenterMock = Substitute.For<IQueryResultPresenter>();

        var countOfMatchedRows = 0;
        presenterMock.When(x => x.PresentResults(Arg.Any<System.Data.IDataReader>())).Do(callInfo =>
        {
            var systemDataReader = callInfo.ArgAt<IDataReader>(0);
            while (systemDataReader.Read())
            {
                countOfMatchedRows++;
            }
        });

        var databaseQueryRunner = new DatabaseQueryRunner(new SqlServerConnectionFactory(connectionString), new SqlServerCommandFactory(), new SqlServerQueryParameterBinder(sqlServerSyntaxFormatter), presenterMock);
        var queryExecutionOrchestrator = new QueryExecutionOrchestrator(sqlQueryCompiler, databaseQueryRunner);
        var query = new Query()
            .From("student")
            .Select("firstname")
            .Where("ismale", true);

        //act
        queryExecutionOrchestrator.ExecuteQuery(query);

        //assert
        countOfMatchedRows.Should().Be(2);
    }

    [Fact]
    public async Task ExecuteQuery_ShouldReturnCorrectRows_WhenDecimalConditionIsProvided()
    {
        //arrange
        var connectionString = _fixture.ConnectionString;
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        await using var createCommand = connection.CreateCommand();
        createCommand.CommandText = @"
            DROP TABLE IF EXISTS student;
            CREATE TABLE student(
                studentnumber INT PRIMARY KEY,
                firstname VARCHAR(100),
                ismale BIT,
                grade DECIMAL(18, 2)
            );
            INSERT INTO student(studentnumber, firstname, ismale, grade) VALUES (1, 'Amir', 'true', 18.24), (2, 'Mahdi', 'true', 19.24), (3, 'Zahra', 'false', 19.24), (4, 'sama', 'false', 19.24), (5, 'mohammad', 'true', 19.22)";

        await createCommand.ExecuteNonQueryAsync();

        var sqlServerSyntaxFormatter = new SqlServerSyntaxFormatter();
        var sqlQueryCompiler = new SqlQueryCompiler(new SelectClauseBuilder(sqlServerSyntaxFormatter), new FromClauseBuilder(sqlServerSyntaxFormatter), new WhereClauseBuilder(new WhereConditionProcessor(sqlServerSyntaxFormatter)));
        var presenterMock = Substitute.For<IQueryResultPresenter>();

        var countOfMatchedRows = 0;
        presenterMock.When(x => x.PresentResults(Arg.Any<System.Data.IDataReader>())).Do(callInfo =>
        {
            var systemDataReader = callInfo.ArgAt<IDataReader>(0);
            while (systemDataReader.Read())
            {
                countOfMatchedRows++;
            }
        });

        var databaseQueryRunner = new DatabaseQueryRunner(new SqlServerConnectionFactory(connectionString), new SqlServerCommandFactory(), new SqlServerQueryParameterBinder(sqlServerSyntaxFormatter), presenterMock);
        var queryExecutionOrchestrator = new QueryExecutionOrchestrator(sqlQueryCompiler, databaseQueryRunner);
        var query = new Query()
            .From("student")
            .Select("firstname")
            .Where("grade", 19.24m);

        //act
        queryExecutionOrchestrator.ExecuteQuery(query);

        //assert
        countOfMatchedRows.Should().Be(3);
    }

    [Fact]
    public async Task ExecuteQuery_ShouldReturnCorrectData_WhenAllTheWhereConditionsProvided()
    {
        //arrange
        var connectionString = _fixture.ConnectionString;
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        await using var createCommand = connection.CreateCommand();
        createCommand.CommandText = @"
            DROP TABLE IF EXISTS student;
            CREATE TABLE student(
                studentnumber INT PRIMARY KEY,
                firstname VARCHAR(100),
                ismale BIT,
                grade DECIMAL(18, 2)
            );
            INSERT INTO student(studentnumber, firstname, ismale, grade) VALUES (1, 'Amir', 'true', 18.24), (2, 'Mahdi', 'true', 19.24), (3, 'Zahra', 'false', 19.24)";
        await createCommand.ExecuteNonQueryAsync();

        var sqlServerSyntaxFormatter = new SqlServerSyntaxFormatter();
        var sqlQueryCompiler = new SqlQueryCompiler(new SelectClauseBuilder(sqlServerSyntaxFormatter), new FromClauseBuilder(sqlServerSyntaxFormatter), new WhereClauseBuilder(new WhereConditionProcessor(sqlServerSyntaxFormatter)));
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

        var databaseQueryRunner = new DatabaseQueryRunner(new SqlServerConnectionFactory(connectionString), new SqlServerCommandFactory(), new SqlServerQueryParameterBinder(sqlServerSyntaxFormatter), presenterMock);
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

    [Fact]
    public async Task ExecuteQuery_ShouldReturnZeroRows_WhenNoMatchingDataExists()
    {
        //arrange
        var connectionString = _fixture.ConnectionString;
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        await using var createCommand = connection.CreateCommand();
        createCommand.CommandText = @"
            DROP TABLE IF EXISTS student;
            CREATE TABLE student(
                studentnumber INT PRIMARY KEY,
                firstname VARCHAR(100),
                ismale BIT,
                grade DECIMAL(18, 2)
            );
            INSERT INTO student(studentnumber, firstname, ismale, grade) VALUES (1, 'Amir', 'true', 18.24), (2, 'Mahdi', 'true', 19.24), (3, 'Zahra', 'false', 19.24), (4, 'sama', 'false', 19.24), (5, 'mohammad', 'true', 19.22)";

        await createCommand.ExecuteNonQueryAsync();

        var sqlServerSyntaxFormatter = new SqlServerSyntaxFormatter();
        var sqlQueryCompiler = new SqlQueryCompiler(new SelectClauseBuilder(sqlServerSyntaxFormatter), new FromClauseBuilder(sqlServerSyntaxFormatter), new WhereClauseBuilder(new WhereConditionProcessor(sqlServerSyntaxFormatter)));
        var presenterMock = Substitute.For<IQueryResultPresenter>();

        var countOfMatchedRows = 0;
        presenterMock.When(x => x.PresentResults(Arg.Any<System.Data.IDataReader>())).Do(callInfo =>
        {
            var systemDataReader = callInfo.ArgAt<IDataReader>(0);
            while (systemDataReader.Read())
            {
                countOfMatchedRows++;
            }
        });

        var databaseQueryRunner = new DatabaseQueryRunner(new SqlServerConnectionFactory(connectionString), new SqlServerCommandFactory(), new SqlServerQueryParameterBinder(sqlServerSyntaxFormatter), presenterMock);
        var queryExecutionOrchestrator = new QueryExecutionOrchestrator(sqlQueryCompiler, databaseQueryRunner);
        var query = new Query()
            .From("student")
            .Select("firstname")
            .Where("grade", 100m);

        //act
        queryExecutionOrchestrator.ExecuteQuery(query);

        //assert
        countOfMatchedRows.Should().Be(0);
    }

    [Fact]
    public async Task ExecuteQuery_ShouldExecuteWithoutError_WhenWhereConditionIsNull()
    {
        //arrange
        var connectionString = _fixture.ConnectionString;
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        await using var createCommand = connection.CreateCommand();
        createCommand.CommandText = @"
            DROP TABLE IF EXISTS student;
            CREATE TABLE student(
                studentnumber INT PRIMARY KEY,
                firstname VARCHAR(100),
                ismale BIT,
                grade DECIMAL(18, 2)
            );
            INSERT INTO student(studentnumber, firstname, ismale, grade) VALUES 
            (1, 'Amir', 'true', 18.24), 
            (2, NULL, 'true', 19.24);";
            
        await createCommand.ExecuteNonQueryAsync();
        
        var sqlServerSyntaxFormatter = new SqlServerSyntaxFormatter();
        var sqlQueryCompiler = new SqlQueryCompiler(
            new SelectClauseBuilder(sqlServerSyntaxFormatter),
            new FromClauseBuilder(sqlServerSyntaxFormatter),
            new WhereClauseBuilder(new WhereConditionProcessor(sqlServerSyntaxFormatter))
        );
        
        var presenterMock = Substitute.For<IQueryResultPresenter>();
        var countOfMatchedRows = 0;
    
        presenterMock.When(x => x.PresentResults(Arg.Any<System.Data.IDataReader>())).Do(callInfo =>
        {
            var systemDataReader = callInfo.ArgAt<System.Data.IDataReader>(0);
            while (systemDataReader.Read())
            {
                countOfMatchedRows++;
            }
        });

        var databaseQueryRunner = new DatabaseQueryRunner(
            new SqlServerConnectionFactory(connectionString), 
            new SqlServerCommandFactory(), 
            new SqlServerQueryParameterBinder(sqlServerSyntaxFormatter), 
            presenterMock
        );

        var queryExecutionOrchestrator = new QueryExecutionOrchestrator(sqlQueryCompiler, databaseQueryRunner);
    
        var query = new Query()
            .From("student")
            .Select("studentnumber")
            .Where("firstname", null);
            
        //act
        var nullAct = () => queryExecutionOrchestrator.ExecuteQuery(query);
        
        //assert
        nullAct.Should().NotThrow();
        countOfMatchedRows.Should().Be(0);
    }
    [Fact]
    public void ExecuteQuery_ShouldThrowInvalidOperationException_WhenDatabaseThrowsException()
    {
        //arrange
        var connectionString = _fixture.ConnectionString;

        var sqlServerSyntaxFormatter = new SqlServerSyntaxFormatter();
        var sqlQueryCompiler = new SqlQueryCompiler(
            new SelectClauseBuilder(sqlServerSyntaxFormatter),
            new FromClauseBuilder(sqlServerSyntaxFormatter),
            new WhereClauseBuilder(new WhereConditionProcessor(sqlServerSyntaxFormatter))
        );

        var presenterMock = Substitute.For<IQueryResultPresenter>();

        var databaseQueryRunner = new DatabaseQueryRunner(
            new SqlServerConnectionFactory(connectionString), 
            new SqlServerCommandFactory(), 
            new SqlServerQueryParameterBinder(sqlServerSyntaxFormatter), 
            presenterMock
        );

        var queryExecutionOrchestrator = new QueryExecutionOrchestrator(sqlQueryCompiler, databaseQueryRunner);
        
        var query = new Query()
            .From("table_that_does_not_exist")
            .Select("some_column");

        //act
        var act = () => queryExecutionOrchestrator.ExecuteQuery(query);

        //assert
        act.Should().Throw<InvalidOperationException>();
    }
    [Fact]
    public async Task ExecuteQuery_ShouldReturnAllColumns_WhenNoSelectClauseIsProvided()
    {
        //arrange
        var connectionString = _fixture.ConnectionString;
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        await using var createCommand = connection.CreateCommand();
        createCommand.CommandText = @"
            DROP TABLE IF EXISTS student;
            CREATE TABLE student(
                studentnumber INT PRIMARY KEY,
                firstname VARCHAR(100),
                ismale BIT,
                grade DECIMAL(18, 2)
            );
            INSERT INTO student(studentnumber, firstname, ismale, grade) VALUES (1, 'Amir', 'true', 18.24)";
        await createCommand.ExecuteNonQueryAsync();

        var sqlServerSyntaxFormatter = new SqlServerSyntaxFormatter();
        var sqlQueryCompiler = new SqlQueryCompiler(new SelectClauseBuilder(sqlServerSyntaxFormatter), new FromClauseBuilder(sqlServerSyntaxFormatter), new WhereClauseBuilder(new WhereConditionProcessor(sqlServerSyntaxFormatter)));
        var presenterMock = Substitute.For<IQueryResultPresenter>();
        
        int fieldCount = 0;
        presenterMock.When(x => x.PresentResults(Arg.Any<System.Data.IDataReader>())).Do(callInfo =>
        {
            var systemDataReader = callInfo.ArgAt<System.Data.IDataReader>(0);
            fieldCount = systemDataReader.FieldCount;
            while (systemDataReader.Read()) { }
        });

        var databaseQueryRunner = new DatabaseQueryRunner(new SqlServerConnectionFactory(connectionString), new SqlServerCommandFactory(), new SqlServerQueryParameterBinder(sqlServerSyntaxFormatter), presenterMock);
        var queryExecutionOrchestrator = new QueryExecutionOrchestrator(sqlQueryCompiler, databaseQueryRunner);
        
        var query = new Query().From("student");

        //act
        queryExecutionOrchestrator.ExecuteQuery(query);

        //assert
        fieldCount.Should().Be(4); 
    }

    [Fact]
    public async Task ExecuteQuery_ShouldReturnCorrectRows_WhenStringConditionIsProvided()
    {
        //arrange
        var connectionString = _fixture.ConnectionString;
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        await using var createCommand = connection.CreateCommand();
        createCommand.CommandText = @"
            DROP TABLE IF EXISTS student;
            CREATE TABLE student(
                studentnumber INT PRIMARY KEY,
                firstname VARCHAR(100)
            );
            INSERT INTO student(studentnumber, firstname) VALUES (1, 'Amir'), (2, 'Mahdi'), (3, 'Zahra')";
        await createCommand.ExecuteNonQueryAsync();

        var sqlServerSyntaxFormatter = new SqlServerSyntaxFormatter();
        var sqlQueryCompiler = new SqlQueryCompiler(new SelectClauseBuilder(sqlServerSyntaxFormatter), new FromClauseBuilder(sqlServerSyntaxFormatter), new WhereClauseBuilder(new WhereConditionProcessor(sqlServerSyntaxFormatter)));
        var presenterMock = Substitute.For<IQueryResultPresenter>();
        
        var countOfMatchedRows = 0;
        presenterMock.When(x => x.PresentResults(Arg.Any<System.Data.IDataReader>())).Do(callInfo =>
        {
            var systemDataReader = callInfo.ArgAt<System.Data.IDataReader>(0);
            while (systemDataReader.Read()) countOfMatchedRows++;
        });

        var databaseQueryRunner = new DatabaseQueryRunner(new SqlServerConnectionFactory(connectionString), new SqlServerCommandFactory(), new SqlServerQueryParameterBinder(sqlServerSyntaxFormatter), presenterMock);
        var queryExecutionOrchestrator = new QueryExecutionOrchestrator(sqlQueryCompiler, databaseQueryRunner);
        
        var query = new Query().From("student").Select("studentnumber").Where("firstname", "Amir");

        //act
        queryExecutionOrchestrator.ExecuteQuery(query);

        //assert
        countOfMatchedRows.Should().Be(1);
    }

    [Fact]
    public async Task ExecuteQuery_ShouldHandleSpacesInTableAndColumnNames_WhenFormatIdentifierIsUsed()
    {
        //arrange
        var connectionString = _fixture.ConnectionString;
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        await using var createCommand = connection.CreateCommand();
        
        createCommand.CommandText = @"
            DROP TABLE IF EXISTS [my students];
            CREATE TABLE [my students](
                [student id] INT PRIMARY KEY,
                [first name] VARCHAR(100)
            );
            INSERT INTO [my students]([student id], [first name]) VALUES (1, 'Amir')";
        await createCommand.ExecuteNonQueryAsync();

        var sqlServerSyntaxFormatter = new SqlServerSyntaxFormatter();
        var sqlQueryCompiler = new SqlQueryCompiler(new SelectClauseBuilder(sqlServerSyntaxFormatter), new FromClauseBuilder(sqlServerSyntaxFormatter), new WhereClauseBuilder(new WhereConditionProcessor(sqlServerSyntaxFormatter)));
        var presenterMock = Substitute.For<IQueryResultPresenter>();
        
        var countOfMatchedRows = 0;
        presenterMock.When(x => x.PresentResults(Arg.Any<System.Data.IDataReader>())).Do(callInfo =>
        {
            var systemDataReader = callInfo.ArgAt<System.Data.IDataReader>(0);
            while (systemDataReader.Read()) countOfMatchedRows++;
        });

        var databaseQueryRunner = new DatabaseQueryRunner(new SqlServerConnectionFactory(connectionString), new SqlServerCommandFactory(), new SqlServerQueryParameterBinder(sqlServerSyntaxFormatter), presenterMock);
        var queryExecutionOrchestrator = new QueryExecutionOrchestrator(sqlQueryCompiler, databaseQueryRunner);
        
        var query = new Query().From("my students").Select("first name").Where("first name", "Amir");

        //act
        queryExecutionOrchestrator.ExecuteQuery(query);

        //assert
        countOfMatchedRows.Should().Be(1);
    }
}
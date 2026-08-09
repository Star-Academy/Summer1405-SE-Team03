using System;
using System.Collections.Generic;
using System.Data.Common;
using FluentAssertions;
using MyNewProjectName.Core;
using MyNewProjectName.Execution.Abstractions;
using MyNewProjectName.Execution.Business;
using MyNewProjectName.Presentation.Abstractions;
using NSubstitute;
using Xunit;

namespace MyNewProjectName.Tests.Execution;

public class DatabaseQueryRunnerTests
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly IDbCommandFactory _dbCommandFactory;
    private readonly IQueryParameterBinder _queryParameterBinder;
    private readonly IQueryResultPresenter _queryResultPresenter;
    private readonly DatabaseQueryRunner _sut;

    public DatabaseQueryRunnerTests()
    {
        _dbConnectionFactory = Substitute.For<IDbConnectionFactory>();
        _dbCommandFactory = Substitute.For<IDbCommandFactory>();
        _queryParameterBinder = Substitute.For<IQueryParameterBinder>();
        _queryResultPresenter = Substitute.For<IQueryResultPresenter>();
        
        _sut = new DatabaseQueryRunner(_dbConnectionFactory, _dbCommandFactory, _queryParameterBinder, _queryResultPresenter);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenDbConnectionFactoryIsNull()
    {
        // Arrange & Act
        var action = () => new DatabaseQueryRunner(null!, _dbCommandFactory, _queryParameterBinder, _queryResultPresenter);

        // Assert
        action.Should().Throw<ArgumentNullException>()
            .WithParameterName("dbConnectionFactory");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenDbCommandFactoryIsNull()
    {
        // Arrange & Act
        var action = () => new DatabaseQueryRunner(_dbConnectionFactory, null!, _queryParameterBinder, _queryResultPresenter);

        // Assert
        action.Should().Throw<ArgumentNullException>()
            .WithParameterName("dbCommandFactory");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenQueryParameterBinderIsNull()
    {
        // Arrange & Act
        var action = () => new DatabaseQueryRunner(_dbConnectionFactory, _dbCommandFactory, null!, _queryResultPresenter);

        // Assert
        action.Should().Throw<ArgumentNullException>()
            .WithParameterName("queryParameterBinder");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenQueryResultPresenterIsNull()
    {
        // Arrange & Act
        var action = () => new DatabaseQueryRunner(_dbConnectionFactory, _dbCommandFactory, _queryParameterBinder, null!);

        // Assert
        action.Should().Throw<ArgumentNullException>()
            .WithParameterName("queryResultPresenter");
    }

    [Fact]
    public void QueryRunner_ShouldExecuteQueryAndPresentResults_WhenQueryHasNoParameters()
    {
        // Arrange
        var originalQuery = new Query().From("student");
        var compiledQuery = new CompiledQuery("SELECT * FROM \"student\"");

        var connection = Substitute.For<DbConnection>();
        var command = Substitute.For<DbCommand>();
        var reader = Substitute.For<DbDataReader>();

        _dbConnectionFactory.CreateConnection().Returns(connection);
        
        _dbCommandFactory.CreateCommand(compiledQuery.SqlQuery, connection).Returns(command);
        
        command.ExecuteReader().Returns(reader);
        _queryParameterBinder.BindParameters(originalQuery).Returns(new List<QueryParameter>());

        // Act
        _sut.QueryRunner(compiledQuery, originalQuery);

        // Assert
        _queryResultPresenter.Received(1).PresentResults(reader);
    }

    [Fact]
    public void QueryRunner_ShouldAddParametersToCommand_WhenQueryHasParameters()
    {
        // Arrange
        var originalQuery = new Query().From("student").Where("grade", 19.5);
        var compiledQuery = new CompiledQuery("SELECT * FROM \"student\" WHERE \"grade\" = $1");

        var connection = Substitute.For<DbConnection>();
        var command = Substitute.For<DbCommand>();
        var reader = Substitute.For<DbDataReader>();
        var parameterCollection = Substitute.For<DbParameterCollection>();
        var dbParameter = Substitute.For<DbParameter>();

        _dbConnectionFactory.CreateConnection().Returns(connection);
        _dbCommandFactory.CreateCommand(compiledQuery.SqlQuery, connection).Returns(command);
        command.Parameters.Returns(parameterCollection); 
        command.CreateParameter().Returns(dbParameter);
        command.ExecuteReader().Returns(reader);

        var parameters = new List<QueryParameter>
        {
            new QueryParameter("$1", 19.5)
        };
        _queryParameterBinder.BindParameters(originalQuery).Returns(parameters);

        // Act
        _sut.QueryRunner(compiledQuery, originalQuery);

        // Assert
        dbParameter.ParameterName.Should().Be("$1");
        dbParameter.Value.Should().Be(19.5);
        parameterCollection.Received(1).Add(dbParameter);
        _queryResultPresenter.Received(1).PresentResults(reader);
    }
    
    [Fact]
    public void QueryRunner_ShouldConvertNullParameterValueToDBNull_WhenParameterValueIsNull()
    {
        // Arrange
        var originalQuery = new Query().From("student").Where("firstname", null!);
        var compiledQuery = new CompiledQuery("SELECT * FROM \"student\" WHERE \"firstname\" = $1");

        var connection = Substitute.For<DbConnection>();
        var command = Substitute.For<DbCommand>();
        var reader = Substitute.For<DbDataReader>();
        var parameterCollection = Substitute.For<DbParameterCollection>();
        var dbParameter = Substitute.For<DbParameter>();

        _dbConnectionFactory.CreateConnection().Returns(connection);
        _dbCommandFactory.CreateCommand(compiledQuery.SqlQuery, connection).Returns(command);
        command.Parameters.Returns(parameterCollection);
        command.CreateParameter().Returns(dbParameter);
        command.ExecuteReader().Returns(reader);

        var parameters = new List<QueryParameter>
        {
            new QueryParameter("$1", null)
        };
        _queryParameterBinder.BindParameters(originalQuery).Returns(parameters);

        // Act
        _sut.QueryRunner(compiledQuery, originalQuery);

        // Assert
        dbParameter.ParameterName.Should().Be("$1");
        dbParameter.Value.Should().Be(DBNull.Value);
        parameterCollection.Received(1).Add(dbParameter);
    }

    [Fact]
    public void QueryRunner_ShouldThrowInvalidOperationException_WhenDbExceptionOccurs()
    {
        // Arrange
        var originalQuery = new Query().From("student");
        var compiledQuery = new CompiledQuery("SELECT * FROM \"student\"");

        var connection = Substitute.For<DbConnection>();
        var dbException = Substitute.For<DbException>();
        dbException.Message.Returns("Connection failed");

        _dbConnectionFactory.CreateConnection().Returns(connection);
        connection.When(x => x.Open()).Do(x => throw dbException);

        // Act
        var action = () => _sut.QueryRunner(compiledQuery, originalQuery);

        // Assert
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("Database Execution Error: Connection failed") 
            .WithInnerException<DbException>()
            .WithMessage("Connection failed"); 
    }
}
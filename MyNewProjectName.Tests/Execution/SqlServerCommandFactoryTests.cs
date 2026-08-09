using System;
using System.Data;
using FluentAssertions;
using MyNewProjectName.Execution.Business;
using NSubstitute;
using Microsoft.Data.SqlClient;
using Xunit;

namespace MyNewProjectName.Tests.Execution;

public class SqlServerCommandFactoryTests
{
    private readonly SqlServerCommandFactory _sut;

    public SqlServerCommandFactoryTests()
    {
        _sut = new SqlServerCommandFactory();
    }

    [Fact]
    public void CreateCommand_ShouldThrowArgumentNullException_WhenConnectionIsNull()
    {
        //arrange
        var sqlText = "SELECT * FROM student";

        //act
        Action act = () => _sut.CreateCommand(sqlText, null!);

        //assert
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("dbConnection");
    }
    
    [Fact]
    public void CreateCommand_ShouldThrowInvalidOperationException_WhenConnectionIsNotSqlConnection()
    {
        //arrange
        var invalidConnection = Substitute.For<IDbConnection>();
        var sqlText = "SELECT * FROM student";

        //act
        Action act = () => _sut.CreateCommand(sqlText, invalidConnection);

        //assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Connection must be of type SqlConnection.");
    }

    [Fact]
    public void CreateCommand_ShouldReturnSqlCommand_WhenConnectionIsSqlConnection()
    {
        //arrange
        var sqlText = "SELECT * FROM student";
        
        using var validConnection = new SqlConnection("Server=localhost;Database=my_db;User Id=sa;Password=pass;TrustServerCertificate=True;");

        //act
        var result = _sut.CreateCommand(sqlText, validConnection);

        //assert
        result.Should().NotBeNull();
        result.Should().BeOfType<SqlCommand>();
        result.CommandText.Should().Be(sqlText);
        result.Connection.Should().Be(validConnection);
    }
}
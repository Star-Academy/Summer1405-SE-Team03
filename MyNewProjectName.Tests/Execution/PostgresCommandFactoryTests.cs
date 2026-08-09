using System;
using System.Data;
using FluentAssertions;
using MyNewProjectName.Execution.Business;
using NSubstitute;
using Npgsql;
using Xunit;

namespace MyNewProjectName.Tests.Execution;

public class PostgresCommandFactoryTests
{
    private readonly PostgresCommandFactory _sut;

    public PostgresCommandFactoryTests()
    {
        _sut = new PostgresCommandFactory();
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
    public void CreateCommand_ShouldThrowInvalidOperationException_WhenConnectionIsNotNpgsqlConnection()
    {
        //arrange
        var invalidConnection = Substitute.For<IDbConnection>();
        var sqlText = "SELECT * FROM student";

        //act
        Action act = () => _sut.CreateCommand(sqlText, invalidConnection);

        //assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Connection must be of type NpgsqlConnection.");
    }

    [Fact]
    public void CreateCommand_ShouldReturnNpgsqlCommand_WhenConnectionIsNpgsqlConnection()
    {
        //arrange
        var sqlText = "SELECT * FROM student";
        
        using var validConnection = new NpgsqlConnection("Host=localhost;Database=my_db;");

        //act
        var result = _sut.CreateCommand(sqlText, validConnection);

        //assert
        result.Should().NotBeNull();
        result.Should().BeOfType<NpgsqlCommand>();
        result.CommandText.Should().Be(sqlText);
        result.Connection.Should().Be(validConnection);
    }
}
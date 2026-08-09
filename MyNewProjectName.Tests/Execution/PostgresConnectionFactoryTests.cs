using Xunit.Abstractions;
using System;
using System.Data;
using FluentAssertions;
using MyNewProjectName.Execution.Business;
using Npgsql;
using Xunit;
namespace MyNewProjectName.Tests.Execution;

public class PostgresConnectionFactoryTests
{
    private readonly PostgresConnectionFactory _sut;
    private readonly string _validConnectionString;

    public PostgresConnectionFactoryTests()
    {
        _validConnectionString = "Host=localhost;Database=my_db;Username=postgres;Password=pass";
        _sut = new PostgresConnectionFactory(_validConnectionString);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenConnectionStringIsNull()
    {
        //act
        var act = () => new PostgresConnectionFactory(null!);
        //assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("connectionString");
    }
    
    [Fact]
    public void CreateConnection_ShouldReturnNpgsqlConnection_WhenWeHaveCorrectConnectionString()
    {
        //act
        var correctResult = _sut.CreateConnection();
        //assert
        correctResult.Should().NotBeNull();
        correctResult.Should().BeOfType<NpgsqlConnection>();
        correctResult.ConnectionString.Should().Be(_validConnectionString);
    }
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_ShouldThrowArgumentException_WhenConnectionStringIsEmptyOrWhitespace(string invalidConnectionString)
    {
        //act
        var emptyAct = () => new PostgresConnectionFactory(invalidConnectionString);
        //assert
        emptyAct.Should().Throw<ArgumentException>();
    }
}
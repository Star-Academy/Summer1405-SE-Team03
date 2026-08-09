using Xunit.Abstractions;
using System;
using System.Data;
using FluentAssertions;
using MyNewProjectName.Execution.Business;
using Microsoft.Data.SqlClient;
using Xunit;
namespace MyNewProjectName.Tests.Execution;

public class SqlServerConnectionFactoryTests
{
    private readonly SqlServerConnectionFactory _sut;
    private readonly string _validConnectionString;

    public SqlServerConnectionFactoryTests()
    {
        _validConnectionString = "Server=localhost;Database=my_db;User Id=sa;Password=pass;TrustServerCertificate=True;";
        _sut = new SqlServerConnectionFactory(_validConnectionString);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenConnectionStringIsNull()
    {
        //act
        var act = () => new SqlServerConnectionFactory(null!);
        //assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("connectionString");
    }
    
    [Fact]
    public void CreateConnection_ShouldReturnSqlConnection_WhenWeHaveCorrectConnectionString()
    {
        //act
        var correctResult = _sut.CreateConnection();
        //assert
        correctResult.Should().NotBeNull();
        correctResult.Should().BeOfType<SqlConnection>();
        correctResult.ConnectionString.Should().Be(_validConnectionString);
    }
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_ShouldThrowArgumentException_WhenConnectionStringIsEmptyOrWhitespace(string invalidConnectionString)
    {
        //act
        var emptyAct = () => new SqlServerConnectionFactory(invalidConnectionString);
        //assert
        emptyAct.Should().Throw<ArgumentException>();
    }
}
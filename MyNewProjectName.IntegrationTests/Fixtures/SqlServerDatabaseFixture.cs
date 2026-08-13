using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Testcontainers.MsSql;
using Xunit;

namespace MyNewProjectName.IntegrationTests.Fixtures;

public sealed class SqlServerDatabaseFixture : IAsyncLifetime
{
    private MsSqlContainer MsSqlContainer { get; set; } = null!;
    public string ConnectionString => MsSqlContainer.GetConnectionString();

    public async Task InitializeAsync()
    {
        MsSqlContainer = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-latest")
            .WithPassword("Secret123!")
            .Build();

        await MsSqlContainer.StartAsync();
        await SeedDatabaseAsync();
    }

    private async Task SeedDatabaseAsync()
    {
        await using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();
        await using var command = connection.CreateCommand();

        command.CommandText = @"
            DROP TABLE IF EXISTS student;
            DROP TABLE IF EXISTS [my students];

            CREATE TABLE student(
                studentnumber INT PRIMARY KEY,
                firstname VARCHAR(100),
                ismale BIT,
                grade DECIMAL(18, 2)
            );

            INSERT INTO student(studentnumber, firstname, ismale, grade) VALUES 
            (1, 'Amir', 1, 18.24), 
            (2, 'Mahdi', 1, 19.24), 
            (3, 'Zahra', 0, 19.24),
            (4, 'sama', 0, 19.24),
            (5, 'mohammad', 1, 19.22),
            (6, NULL, 0, 15.00);

            CREATE TABLE [my students](
                [student id] INT PRIMARY KEY,
                [first name] VARCHAR(100)
            );

            INSERT INTO [my students]([student id], [first name]) VALUES 
            (1, 'Amir');";

        await command.ExecuteNonQueryAsync();
    }

    public async Task DisposeAsync()
    {
        await MsSqlContainer.DisposeAsync();
    }
}
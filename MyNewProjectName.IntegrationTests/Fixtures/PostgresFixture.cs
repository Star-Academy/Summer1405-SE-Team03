using System.Threading.Tasks;
using Npgsql;
using Testcontainers.PostgreSql;
using Xunit;

namespace MyNewProjectName.IntegrationTests.Fixtures;

public sealed class PostgresDatabaseFixture : IAsyncLifetime
{
    private PostgreSqlContainer PostgreSqlContainer { get; set; } = null!;
    public string ConnectionString => PostgreSqlContainer.GetConnectionString();

    public async Task InitializeAsync()
    {
        PostgreSqlContainer = new PostgreSqlBuilder("postgres:16-alpine")
            .WithDatabase("psgDB")
            .WithUsername("psg")
            .WithPassword("psg")
            .Build();

        await PostgreSqlContainer.StartAsync();
        await SeedDatabaseAsync();
    }

    private async Task SeedDatabaseAsync()
    {
        await using var connection = new NpgsqlConnection(ConnectionString);
        await connection.OpenAsync();
        await using var command = connection.CreateCommand();

        command.CommandText = @"
            DROP TABLE IF EXISTS student;
            DROP TABLE IF EXISTS ""my students"";

            CREATE TABLE student(
                studentnumber INT PRIMARY KEY,
                firstname VARCHAR(100),
                ismale BOOLEAN,
                grade DECIMAL
            );

            INSERT INTO student(studentnumber, firstname, ismale, grade) VALUES 
            (1, 'Amir', true, 18.24), 
            (2, 'Mahdi', true, 19.24), 
            (3, 'Zahra', false, 19.24),
            (4, 'sama', false, 19.24),
            (5, 'mohammad', true, 19.22),
            (6, NULL, false, 15.00);

            CREATE TABLE ""my students""(
                ""student id"" INT PRIMARY KEY,
                ""first name"" VARCHAR(100)
            );

            INSERT INTO ""my students""(""student id"", ""first name"") VALUES 
            (1, 'Amir');";

        await command.ExecuteNonQueryAsync();
    }

    public async Task DisposeAsync()
    {
        await PostgreSqlContainer.DisposeAsync();
    }
}
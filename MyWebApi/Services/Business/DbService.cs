using Microsoft.Data.SqlClient;
using MyWebApi.Exceptions;
using MyWebApi.Models;
using MyWebApi.Services.Abstractions;
using Npgsql;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace MyWebApi.Services.Business;

public class DbService : IDbService
{
    private const string PostgresConnectionName = "PostgresConnection";
    private const string SqlServerConnectionName = "SqlServerConnection";
    private const int QueryTimeoutSeconds = 30;

    private readonly IConfiguration _configuration;

    public DbService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public QueryFactory GetQueryFactory(string? dbType)
    {
        var databaseType = ParseDatabaseType(dbType);

        return databaseType switch
        {
            DatabaseType.Postgres => CreatePostgresQueryFactory(),
            DatabaseType.SqlServer => CreateSqlServerQueryFactory(),
            _ => throw new InvalidDatabaseException("dbType is not valid.")
        };
    }

    private static DatabaseType ParseDatabaseType(string? dbType)
    {
        if (string.IsNullOrWhiteSpace(dbType))
        {
            throw new InvalidDatabaseException("dbType is required.");
        }

        return dbType.Trim().ToLowerInvariant() switch
        {
            "postgres" => DatabaseType.Postgres,
            "sqlserver" => DatabaseType.SqlServer,
            _ => throw new InvalidDatabaseException("dbType is not valid.")
        };
    }

    private QueryFactory CreatePostgresQueryFactory()
    {
        var connectionString = GetConnectionString(PostgresConnectionName);
        var connection = new NpgsqlConnection(connectionString);

        return new QueryFactory(connection, new PostgresCompiler(), QueryTimeoutSeconds);
    }

    private QueryFactory CreateSqlServerQueryFactory()
    {
        var connectionString = GetConnectionString(SqlServerConnectionName);
        var connection = new SqlConnection(connectionString);

        return new QueryFactory(connection, new SqlServerCompiler(), QueryTimeoutSeconds);
    }

    private string GetConnectionString(string connectionName)
    {
        var connectionString = _configuration.GetConnectionString(connectionName);

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException($"{connectionName} is not configured.");
        }

        return connectionString;
    }
}
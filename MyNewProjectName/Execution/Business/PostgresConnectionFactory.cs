using System;
using System.Data;
using MyNewProjectName.Execution.Abstractions;
using Npgsql;

namespace MyNewProjectName.Execution.Business;

internal sealed class PostgresConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public PostgresConnectionFactory(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);
}
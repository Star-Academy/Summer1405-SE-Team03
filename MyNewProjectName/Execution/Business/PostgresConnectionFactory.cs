using System.Data;
using MyNewProjectName.Execution.Abstractions;
using Npgsql;

namespace MyNewProjectName.Execution.Business;

internal sealed class PostgresConnectionFactory(string connectionString) : IDbConnectionFactory
{
    public IDbConnection CreateConnection() => new NpgsqlConnection(connectionString);
}
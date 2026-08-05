using System.Data;
using MyNewProjectName.Execution.Abstractions;
using Npgsql;

namespace MyNewProjectName.Execution.Business;

public class PostgresConnectionFactory : IDbConnectionFactory
{
    public IDbConnection CreateConnection(string connectionString)
        => new NpgsqlConnection(connectionString);
}
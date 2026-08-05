using System.Data;
using Npgsql;

namespace MyNewProjectName;

public class PostgresConnectionFactory : IDbConnectionFactory
{
    public IDbConnection CreateConnection(string connectionString)
        => new NpgsqlConnection(connectionString);
}
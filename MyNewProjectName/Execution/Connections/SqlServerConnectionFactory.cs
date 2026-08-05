using System.Data;
using Microsoft.Data.SqlClient;

namespace MyNewProjectName;

public class SqlServerConnectionFactory : IDbConnectionFactory
{
    public IDbConnection CreateConnection(string connectionString)
        => new SqlConnection(connectionString);
}
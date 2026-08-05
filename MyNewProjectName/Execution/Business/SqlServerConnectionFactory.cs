using System.Data;
using Microsoft.Data.SqlClient;
using MyNewProjectName.Execution.Abstractions;

namespace MyNewProjectName.Execution.Business;

public class SqlServerConnectionFactory : IDbConnectionFactory
{
    public IDbConnection CreateConnection(string connectionString)
        => new SqlConnection(connectionString);
}
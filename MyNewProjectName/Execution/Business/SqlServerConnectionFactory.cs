using System.Data;
using Microsoft.Data.SqlClient;
using MyNewProjectName.Execution.Abstractions;

namespace MyNewProjectName.Execution.Business;

public class SqlServerConnectionFactory(string connectionString) : IDbConnectionFactory
{
    public IDbConnection CreateConnection() => new SqlConnection(connectionString);
}
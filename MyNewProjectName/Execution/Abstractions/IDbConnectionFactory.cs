using System.Data;

namespace MyNewProjectName.Execution.Connections;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection(string connectionString);
}
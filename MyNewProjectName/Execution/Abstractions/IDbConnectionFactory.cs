using System.Data;

namespace MyNewProjectName.Execution.Abstractions;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection(string connectionString);
}
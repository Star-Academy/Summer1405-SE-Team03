using System.Data;

namespace MyNewProjectName;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection(string connectionString);
}
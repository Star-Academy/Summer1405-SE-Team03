using System.Data;

namespace MyNewProjectName;

public interface IDatabaseStrategy
{
    string DatabaseName { get; }
    IDbConnection CreateConnection(string connectionString);
    IDbCommand CreateCommand(string sqlText, IDbConnection connection);
    void AddParameters(IDbCommand cmd, Query query);
}
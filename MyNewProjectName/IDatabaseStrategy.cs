using System.Data;

namespace MyNewProjectName;

public interface IDatabaseStrategy
{
    string DatabaseName { get; }
    IDbConnection CreateConnection(string connectionString);
    void PreExecuteSetup(IDbConnection conn);
    IDbCommand CreateCommand(string sqlText, IDbConnection conn);
    void AddParameters(IDbCommand cmd, Query query);
}
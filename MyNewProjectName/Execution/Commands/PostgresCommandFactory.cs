using System.Data;
using Npgsql;

namespace MyNewProjectName.Execution.Commands;

public class PostgresCommandFactory : IDbCommandFactory
{
    public IDbCommand CreateCommand(string sqlText, IDbConnection dbConnection)
    {
        return dbConnection is not NpgsqlConnection npgsqlConnection ? throw new InvalidOperationException("Connection must be of type NpgsqlConnection.") : new NpgsqlCommand(sqlText, npgsqlConnection);
    }
}
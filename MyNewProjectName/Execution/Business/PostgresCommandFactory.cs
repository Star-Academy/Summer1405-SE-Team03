using System.Data;
using MyNewProjectName.Execution.Abstractions;
using Npgsql;

namespace MyNewProjectName.Execution.Business;

public class PostgresCommandFactory : IDbCommandFactory
{
    public IDbCommand CreateCommand(string sqlText, IDbConnection dbConnection)
    {
        return dbConnection is not NpgsqlConnection npgsqlConnection ? throw new InvalidOperationException("Connection must be of type NpgsqlConnection.") : new NpgsqlCommand(sqlText, npgsqlConnection);
    }
}
using System.Data;
using MyNewProjectName.Execution.Abstractions;
using Npgsql;

namespace MyNewProjectName.Execution.Business;

internal sealed class PostgresCommandFactory : IDbCommandFactory
{
    public IDbCommand CreateCommand(string sqlText, IDbConnection dbConnection)
    {
        ArgumentNullException.ThrowIfNull(dbConnection);
        
        return dbConnection is not NpgsqlConnection npgsqlConnection 
            ? throw new InvalidOperationException("Connection must be of type NpgsqlConnection.") 
            : new NpgsqlCommand(sqlText, npgsqlConnection);
    }
}
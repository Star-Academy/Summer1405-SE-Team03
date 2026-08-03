using System.Data;
using Npgsql;

namespace MyNewProjectName;

public class PostgresStrategy : IDatabaseStrategy
{
    public string DatabaseName => "PostgreSQL";

    public IDbConnection CreateConnection(string connectionString)
    {
        return new NpgsqlConnection(connectionString);
    }
    

    public IDbCommand CreateCommand(string sqlText, IDbConnection connection)
    {
        if (connection is not NpgsqlConnection npgsqlConnection)
        {
            throw new InvalidOperationException("Connection must be of type NpgsqlConnection.");
        }

        return new NpgsqlCommand(sqlText, npgsqlConnection);
    }

    public void AddParameters(IDbCommand cmd, Query query)
    {
        if (cmd is not NpgsqlCommand npgsqlCommand)
        {
            throw new InvalidOperationException("Command must be of type NpgsqlCommand.");
        }

        foreach (var parameter in query.WhereConditions)
        {
            npgsqlCommand.Parameters.Add(new NpgsqlParameter { Value = parameter.Value });
        }
    }
}
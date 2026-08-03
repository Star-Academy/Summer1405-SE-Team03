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

    public void PreExecuteSetup(IDbConnection conn)
    {
        using var schemaCmd = new NpgsqlCommand("SET search_path TO \"TEST-SH\";", (NpgsqlConnection)conn);
        schemaCmd.ExecuteNonQuery();
    }

    public IDbCommand CreateCommand(string sqlText, IDbConnection conn)
    {
        return new NpgsqlCommand(sqlText, (NpgsqlConnection)conn);
    }

    public void AddParameters(IDbCommand cmd, Query query)
    {
        var npgsqlCmd = (NpgsqlCommand)cmd;
        foreach (var param in query.WhereClauses)
        {
            npgsqlCmd.Parameters.Add(new NpgsqlParameter { Value = param.Value });
        }
    }
}
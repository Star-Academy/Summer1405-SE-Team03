using System.Data;
using Microsoft.Data.SqlClient;

namespace MyNewProjectName;

public class SqlServerStrategy : IDatabaseStrategy
{
    public string DatabaseName => "SQL Server";

    public IDbConnection CreateConnection(string connectionString)
    {
        return new SqlConnection(connectionString);
    }

    public void PreExecuteSetup(IDbConnection conn)
    {
    }

    public IDbCommand CreateCommand(string sqlText, IDbConnection conn)
    {
        return new SqlCommand(sqlText, (SqlConnection)conn);
    }

    public void AddParameters(IDbCommand cmd, Query query)
    {
        var sqlCmd = (SqlCommand)cmd;
        int sqlIndex = 0;
        foreach (var param in query.ColumnNameValue)
        {
            sqlCmd.Parameters.AddWithValue($"@p{sqlIndex}", param.Value);
            sqlIndex++;
        }
    }
}
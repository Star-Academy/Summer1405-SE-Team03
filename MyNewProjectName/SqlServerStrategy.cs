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

   

    public IDbCommand CreateCommand(string sqlText, IDbConnection connection)
    {
        return new SqlCommand(sqlText, (SqlConnection)connection);
    }

    public void AddParameters(IDbCommand cmd, Query query)
    {
        var sqlCmd = (SqlCommand)cmd;
        int sqlIndex = 0;
        foreach (var param in query.WhereConditions)
        {
            sqlCmd.Parameters.AddWithValue($"@p{sqlIndex}", param.Value);
            sqlIndex++;
        }
    }
}
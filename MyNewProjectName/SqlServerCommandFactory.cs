using System.Data;
using Microsoft.Data.SqlClient;

namespace MyNewProjectName;

public class SqlServerCommandFactory : IDbCommandFactory
{
    public IDbCommand CreateCommand(string sqlText, IDbConnection dbConnection)
    {
        return dbConnection is not SqlConnection sqlConnection 
            ? throw new InvalidOperationException("Connection must be of type SqlConnection.") 
            : new SqlCommand(sqlText, sqlConnection);
    }
}
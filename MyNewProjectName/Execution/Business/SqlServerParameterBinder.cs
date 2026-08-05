using System.Data;
using Microsoft.Data.SqlClient;
using MyNewProjectName.Core;
using MyNewProjectName.Execution.Abstractions;

namespace MyNewProjectName.Execution.Business;

public class SqlServerParameterBinder : IQueryParameterBinder
{
    public void AddParameters(IDbCommand dbCommand, Query query)
    {
        if (dbCommand is not SqlCommand sqlCommand)
        {
            throw new InvalidOperationException("Command must be of type sqlCommand.");
        }
        var sqlIndex = 0;
        
        foreach (var param in query.WhereConditions)
        {
            sqlCommand.Parameters.AddWithValue($"@p{sqlIndex}", param.Value);
            sqlIndex++;
        }
    }
}
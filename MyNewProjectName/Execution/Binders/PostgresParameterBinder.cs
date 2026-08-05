using System.Data;
using MyNewProjectName.Core;
using Npgsql;

namespace MyNewProjectName.Execution.Binders;

public class PostgresParameterBinder : IQueryParameterBinder
{
    public void AddParameters(IDbCommand dbCommand, Query query)
    {
        if (dbCommand is not NpgsqlCommand npgsqlCommand)
        {
            throw new InvalidOperationException("Command must be of type NpgsqlCommand.");
        }

        foreach (var parameter in query.WhereConditions)
        {
            npgsqlCommand.Parameters.Add(new NpgsqlParameter { Value = parameter.Value });
        }
    }
}
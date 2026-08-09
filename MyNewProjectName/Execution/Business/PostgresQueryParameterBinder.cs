using System;
using System.Collections.Generic;
using MyNewProjectName.Core;
using MyNewProjectName.Execution.Abstractions;
using MyNewProjectName.Grammars.Abstractions;

namespace MyNewProjectName.Execution.Business;

internal sealed class PostgresQueryParameterBinder : IQueryParameterBinder
{
    private readonly IDatabaseSpecificSyntaxFormatter _databaseSpecificSyntaxFormatter;

    public PostgresQueryParameterBinder(IDatabaseSpecificSyntaxFormatter databaseSpecificSyntaxFormatter)
    {
        _databaseSpecificSyntaxFormatter = databaseSpecificSyntaxFormatter ?? throw new ArgumentNullException(nameof(databaseSpecificSyntaxFormatter));
    }

    public IReadOnlyList<QueryParameter> BindParameters(Query query)
    {
        ArgumentNullException.ThrowIfNull(query);

        var resultQueryParameter = new List<QueryParameter>();

        foreach (var parameter in query.WhereConditions)
        {
            resultQueryParameter.Add(new QueryParameter(string.Empty, parameter.Value));
        }

        return resultQueryParameter;
    }
}
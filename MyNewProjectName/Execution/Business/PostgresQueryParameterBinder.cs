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
        var index = _databaseSpecificSyntaxFormatter.ParameterStartIndex;

        foreach (var parameter in query.WhereConditions)
        {
            var bindName = _databaseSpecificSyntaxFormatter.GetParameterName(index);
            resultQueryParameter.Add(new QueryParameter(bindName, parameter.Value));
            index++;
        }
        return resultQueryParameter;
    }
}
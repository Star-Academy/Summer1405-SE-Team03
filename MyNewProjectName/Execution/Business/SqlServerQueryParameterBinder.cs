using System;
using System.Collections.Generic;
using MyNewProjectName.Core;
using MyNewProjectName.Execution.Abstractions;
using MyNewProjectName.Grammars.Abstractions;

namespace MyNewProjectName.Execution.Business;

internal sealed class SqlServerQueryParameterBinder : IQueryParameterBinder
{
    private readonly IDatabaseSpecificSyntaxFormatter _databaseSpecificSyntaxFormatter;

    public SqlServerQueryParameterBinder(IDatabaseSpecificSyntaxFormatter formatter)
    {
        _databaseSpecificSyntaxFormatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
    }

    public IReadOnlyList<QueryParameter> BindParameters(Query query)
    {
        ArgumentNullException.ThrowIfNull(query);

        var resultQueryParameter = new List<QueryParameter>();
        var index = _databaseSpecificSyntaxFormatter.ParameterStartIndex;

        foreach (var parameter in query.WhereConditions)
        {
            if (parameter.Value is null or DBNull)
            {
                continue;
            }
            var paramName = _databaseSpecificSyntaxFormatter.GetParameterName(index);
            resultQueryParameter.Add(new QueryParameter(paramName, parameter.Value));
            index++;
        }

        return resultQueryParameter;
    }
}
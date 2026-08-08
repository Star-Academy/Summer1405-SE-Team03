using System;
using System.Collections.Generic;
using MyNewProjectName.Core;
using MyNewProjectName.Execution.Abstractions;
using MyNewProjectName.Grammars.Abstractions;

namespace MyNewProjectName.Execution.Business;

internal sealed class SqlServerQueryParameterBinder : IQueryParameterBinder
{
    private readonly IDatabaseSpecificSyntaxFormatter _formatter;

    public SqlServerQueryParameterBinder(IDatabaseSpecificSyntaxFormatter formatter)
    {
        _formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
    }

    public IReadOnlyList<QueryParameter> BindParameters(Query query)
    {
        var resultQueryParameter = new List<QueryParameter>();
        var index = _formatter.ParameterStartIndex;

        foreach (var parameter in query.WhereConditions)
        {
            var paramName = _formatter.GetParameterName(index);
            resultQueryParameter.Add(new QueryParameter(paramName, parameter.Value));
            index++;
        }

        return resultQueryParameter;
    }
}
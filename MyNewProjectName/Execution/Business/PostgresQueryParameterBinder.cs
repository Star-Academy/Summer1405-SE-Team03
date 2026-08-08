using System;
using System.Collections.Generic;
using MyNewProjectName.Core;
using MyNewProjectName.Execution.Abstractions;
using MyNewProjectName.Grammars.Abstractions;

namespace MyNewProjectName.Execution.Business;

internal sealed class PostgresQueryParameterBinder : IQueryParameterBinder
{
    private readonly IDatabaseSpecificSyntaxFormatter _formatter;

    public PostgresQueryParameterBinder(IDatabaseSpecificSyntaxFormatter formatter)
    {
        _formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
    }

    public IReadOnlyList<QueryParameter> BindParameters(Query query)
    {
        var resultQueryParameter = new List<QueryParameter>();

        foreach (var parameter in query.WhereConditions)
        {
            resultQueryParameter.Add(new QueryParameter(string.Empty, parameter.Value));
        }

        return resultQueryParameter;
    }
}
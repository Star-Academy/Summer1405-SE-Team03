using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using MyNewProjectName.Compilers.Abstractions;
using MyNewProjectName.Core;

namespace MyNewProjectName.Compilers.Business;

internal sealed class WhereClauseBuilder : IWhereClauseBuilder
{
    private readonly IWhereConditionProcessor _whereConditionProcessor;

    public WhereClauseBuilder(IWhereConditionProcessor whereConditionProcessor)
    {
        _whereConditionProcessor = whereConditionProcessor ?? throw new ArgumentNullException(nameof(whereConditionProcessor));
    }

    public IList<object> Build(StringBuilder queryBuilder, Query query)
    {
        if (!query.WhereConditions.Any()) return new List<object>();

        queryBuilder.Append(" WHERE ");
        var processed = _whereConditionProcessor.Process(query.WhereConditions);

        queryBuilder.Append(string.Join(" AND ", processed.Conditions));
        
        return processed.BindingValues; 
    }
}
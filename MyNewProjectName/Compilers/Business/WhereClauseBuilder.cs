using System;
using System.Collections.Generic;
using System.Linq;
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

    public WhereClauseResult Build(Query query)
    {
        if (!query.WhereConditions.Any()) 
        {
            return new WhereClauseResult(string.Empty);
        }

        var processed = _whereConditionProcessor.Process(query.WhereConditions);
        var sqlText = " WHERE " + string.Join(" AND ", processed.Conditions);

        return new WhereClauseResult(sqlText);
    }
}
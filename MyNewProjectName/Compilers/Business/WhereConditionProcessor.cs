using System;
using System.Collections.Generic;
using MyNewProjectName.Compilers.Abstractions;
using MyNewProjectName.Core;
using MyNewProjectName.Grammars.Abstractions;

namespace MyNewProjectName.Compilers.Business;

internal sealed class WhereConditionProcessor : IWhereConditionProcessor
{
    private readonly IDatabaseSpecificSyntaxFormatter _databaseSpecificSyntaxFormatter;

    public WhereConditionProcessor(IDatabaseSpecificSyntaxFormatter databaseSpecificSyntaxFormatter)
    {
        _databaseSpecificSyntaxFormatter = databaseSpecificSyntaxFormatter ?? throw new ArgumentNullException(nameof(databaseSpecificSyntaxFormatter));
    }

    public ProcessedWhereConditions Process(IList<WhereCondition> clauses)
    {
        var conditions = new List<string>();
        var bindingValues = new List<object>();
        
        var index = _databaseSpecificSyntaxFormatter.ParameterStartIndex;

        foreach (var condition in clauses)
        {
            var paramName = _databaseSpecificSyntaxFormatter.GetParameterName(index);
            conditions.Add($"{_databaseSpecificSyntaxFormatter.FormatIdentifier(condition.ColumnName)} = {paramName}");
            
            bindingValues.Add(condition.Value ?? DBNull.Value);
            index++;
        }

        return new ProcessedWhereConditions(conditions, bindingValues);
    }
}
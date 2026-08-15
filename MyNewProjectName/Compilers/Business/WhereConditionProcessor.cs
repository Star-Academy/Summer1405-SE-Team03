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
        var index = _databaseSpecificSyntaxFormatter.ParameterStartIndex;

        foreach (var condition in clauses)
        {
            var formattedColumn = _databaseSpecificSyntaxFormatter.FormatIdentifier(condition.ColumnName);

            const string nullOp = "IS NULL";
            if (condition.Value is null or DBNull)
            {
                
                conditions.Add($"{formattedColumn} {nullOp}");
            }
            else
            {
                var paramName = _databaseSpecificSyntaxFormatter.GetParameterName(index);
                var op = string.IsNullOrWhiteSpace(condition.Operator) ? "=" : condition.Operator;
                conditions.Add($"{formattedColumn} {op} {paramName}");
                index++;
            }
        }

        return new ProcessedWhereConditions(conditions);
    }
}
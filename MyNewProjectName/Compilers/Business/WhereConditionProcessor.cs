using MyNewProjectName.Core;
using MyNewProjectName.Grammars;

namespace MyNewProjectName.Compilers.Processors;

public class WhereConditionProcessor(ISqlGrammar sqlGrammar) : IWhereConditionProcessor
{
    private readonly ISqlGrammar _sqlGrammar = sqlGrammar ?? throw new ArgumentNullException(nameof(sqlGrammar));

    public (IList<string> Conditions, IList<object> BindingValues) Process(IList<WhereCondition> clauses)
    {
        var conditions = new List<string>();
        var bindingValues = new List<object>();
        
        var index = _sqlGrammar.ParameterStartIndex;

        foreach (var condition in clauses)
        {
            var paramName = _sqlGrammar.GetParameterName(index);
            conditions.Add($"{_sqlGrammar.FormatIdentifier(condition.ColumnName)} = {paramName}");
            
            bindingValues.Add(condition.Value ?? DBNull.Value);
            index++;
        }

        return (conditions, bindingValues);
    }
}
using System.Text;
using MyNewProjectName.Compilers.Processors;
using MyNewProjectName.Core;

namespace MyNewProjectName.Compilers.Builders;
public class WhereClauseBuilder(IWhereConditionProcessor conditionProcessor) : IWhereClauseBuilder
{
    private readonly IWhereConditionProcessor _conditionProcessor = conditionProcessor ?? throw new ArgumentNullException(nameof(conditionProcessor));

    public IList<object> Build(StringBuilder queryBuilder, Query query)
    {
        if (!query.WhereConditions.Any()) return new List<object>();

        queryBuilder.Append(" WHERE ");
        var processed = _conditionProcessor.Process(query.WhereConditions);

        queryBuilder.Append(string.Join(" AND ", processed.Conditions));
        return processed.BindingValues; 
    }
}
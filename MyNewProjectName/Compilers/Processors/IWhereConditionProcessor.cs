using MyNewProjectName.Core;

namespace MyNewProjectName.Compilers.Processors;

public interface IWhereConditionProcessor
{
    (IList<string> Conditions, IList<object> BindingValues) Process(IList<WhereCondition> clauses);
}
using MyNewProjectName.Core;

namespace MyNewProjectName.Compilers.Abstractions;

public interface IWhereConditionProcessor
{
    (IList<string> Conditions, IList<object> BindingValues) Process(IList<WhereCondition> clauses);
}
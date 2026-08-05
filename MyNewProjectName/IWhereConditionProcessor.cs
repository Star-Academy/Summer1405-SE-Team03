using System.Collections.Generic;

namespace MyNewProjectName;

public interface IWhereConditionProcessor
{
    (IList<string> Conditions, IList<object> BindingValues) Process(IList<WhereCondition> clauses);
}
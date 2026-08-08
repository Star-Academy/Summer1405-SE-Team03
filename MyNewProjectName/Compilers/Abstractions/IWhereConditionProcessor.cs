using MyNewProjectName.Compilers.Business;
using MyNewProjectName.Core;

namespace MyNewProjectName.Compilers.Abstractions;

public interface IWhereConditionProcessor
{
    ProcessedWhereConditions Process(IList<WhereCondition> clauses);
}
using MyNewProjectName.Core;

namespace MyNewProjectName.Execution.Abstractions
{
    public interface IQueryExecutor
    {
        void ExecuteQuery(Query query);
    }
}
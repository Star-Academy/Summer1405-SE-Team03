using MyNewProjectName.Core;

namespace MyNewProjectName.Execution
{
    public interface IQueryExecutor
    {
        void ExecuteQuery(Query query);
    }
}
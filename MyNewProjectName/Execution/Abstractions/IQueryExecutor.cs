using System.Data;
using MyNewProjectName.Core;

namespace MyNewProjectName.Execution.Abstractions
{
    public interface IQueryExecutor
    {
        IDataReader ExecuteQuery(Query query);
    }
}
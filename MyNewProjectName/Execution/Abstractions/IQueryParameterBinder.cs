using System.Data;
using MyNewProjectName.Core;

namespace MyNewProjectName.Execution.Binders;

public interface IQueryParameterBinder
{
    void AddParameters(IDbCommand dbCommand, Query query);
}
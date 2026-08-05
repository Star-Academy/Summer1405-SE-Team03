using System.Data;
using MyNewProjectName.Core;

namespace MyNewProjectName.Execution.Abstractions;

public interface IQueryParameterBinder
{
    void AddParameters(IDbCommand dbCommand, Query query);
}
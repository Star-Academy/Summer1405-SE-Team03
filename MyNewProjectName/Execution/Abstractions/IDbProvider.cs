using MyNewProjectName.Execution.Binders;
using MyNewProjectName.Execution.Commands;
using MyNewProjectName.Execution.Connections;

namespace MyNewProjectName;

public interface IDbProvider
{
    IDbConnectionFactory ConnectionFactory { get; }
    IDbCommandFactory CommandFactory { get; }
    IQueryParameterBinder ParameterBinder { get; }
}
using MyNewProjectName.Execution.Binders;
using MyNewProjectName.Execution.Commands;
using MyNewProjectName.Execution.Connections;

namespace MyNewProjectName;

public class PostgresDbProvider : IDbProvider
{
    public IDbConnectionFactory ConnectionFactory { get; } = new PostgresConnectionFactory();
    public IDbCommandFactory CommandFactory { get; } = new PostgresCommandFactory();
    public IQueryParameterBinder ParameterBinder { get; } = new PostgresParameterBinder();
}
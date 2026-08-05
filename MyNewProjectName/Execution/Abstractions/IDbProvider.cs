namespace MyNewProjectName.Execution.Abstractions;

public interface IDbProvider
{
    IDbConnectionFactory ConnectionFactory { get; }
    IDbCommandFactory CommandFactory { get; }
    IQueryParameterBinder ParameterBinder { get; }
}
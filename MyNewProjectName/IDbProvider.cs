namespace MyNewProjectName;

public interface IDbProvider
{
    IDbConnectionFactory ConnectionFactory { get; }
    IDbCommandFactory CommandFactory { get; }
    IQueryParameterBinder ParameterBinder { get; }
}
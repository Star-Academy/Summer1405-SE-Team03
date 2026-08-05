namespace MyNewProjectName;

public class SqlServerDbProvider : IDbProvider
{
    public IDbConnectionFactory ConnectionFactory { get; } = new SqlServerConnectionFactory();
    public IDbCommandFactory CommandFactory { get; } = new SqlServerCommandFactory();
    public IQueryParameterBinder ParameterBinder { get; } = new SqlServerParameterBinder();
}
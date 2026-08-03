namespace MyNewProjectName;

public class PostgresDialect : SqlDialectBase
{
    public PostgresDialect() : base(1) 
    { 
    }
    public override string Quote(string identifier) => $"\"{identifier}\"";
    public override string GetParameterName(int index) => $"${index}";
}
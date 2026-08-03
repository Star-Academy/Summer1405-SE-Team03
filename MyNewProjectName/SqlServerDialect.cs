namespace MyNewProjectName;

public class SqlServerDialect : SqlDialectBase
{
    public SqlServerDialect() : base(0) 
    { 
    }
    public override string Quote(string identifier) => $"[{identifier}]";
    public override string GetParameterName(int index) => $"@p{index}";
}
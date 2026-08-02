namespace MyNewProjectName;

public class SqlServerDialect : ISqlDialect
{
    public int ParameterStartIndex => 0;
    public string Quote(string identifier) => $"[{identifier}]";
    public string GetParameterName(int index) => $"@p{index}";
}
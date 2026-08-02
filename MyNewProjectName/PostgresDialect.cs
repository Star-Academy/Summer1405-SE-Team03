namespace MyNewProjectName;

public class PostgresDialect : ISqlDialect
{
    public int ParameterStartIndex => 1;
    public string Quote(string identifier) => $"\"{identifier}\"";
    public string GetParameterName(int index) => $"${index}";
}
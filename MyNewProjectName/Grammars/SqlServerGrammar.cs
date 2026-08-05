namespace MyNewProjectName;

public class SqlServerGrammar : ISqlGrammar
{
    public int ParameterStartIndex => 0;

    public string FormatIdentifier(string identifier) => $"[{identifier}]";

    public string GetParameterName(int index) => $"@p{index}";
}
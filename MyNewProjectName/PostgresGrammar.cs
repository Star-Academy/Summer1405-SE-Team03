namespace MyNewProjectName;

public class PostgresGrammar : SqlGrammar
{
    public int ParameterStartIndex => 1;

    public string FormatIdentifier(string identifier) => $"\"{identifier}\"";

    public string GetParameterName(int index) => $"${index}";
}
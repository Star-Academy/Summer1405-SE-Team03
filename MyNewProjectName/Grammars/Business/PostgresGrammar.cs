using MyNewProjectName.Grammars.Abstractions;

namespace MyNewProjectName.Grammars.Business;

internal sealed class PostgresGrammar : IDatabaseSpecificSyntaxFormatter
{
    public int ParameterStartIndex => 1;

    public string FormatIdentifier(string identifier) => $"\"{identifier}\"";

    public string GetParameterName(int index) => $"${index}";
}
using MyNewProjectName.Grammars.Abstractions;
namespace MyNewProjectName.Grammars.Business;
using MyNewProjectName.Grammars.Abstractions;
internal sealed class PostgresSyntaxFormatter : IDatabaseSpecificSyntaxFormatter
{
    public int ParameterStartIndex => 1;
    public string FormatIdentifier(string identifier) => $"\"{identifier}\"";
    public string GetParameterName(int index) => $"${index}";
    public string GetBindParameterName(int index) => string.Empty;
}
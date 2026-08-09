using System.Diagnostics.CodeAnalysis;
using MyNewProjectName.Grammars.Abstractions;

namespace MyNewProjectName.Grammars.Business;

[ExcludeFromCodeCoverage]
internal sealed class PostgresSyntaxFormatter : IDatabaseSpecificSyntaxFormatter
{
    public int ParameterStartIndex => 1;
    public string FormatIdentifier(string identifier) => $"\"{identifier}\"";
    public string GetParameterName(int index) => $"@{index}";
    
}
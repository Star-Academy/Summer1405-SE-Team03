using System.Diagnostics.CodeAnalysis;
using MyNewProjectName.Grammars.Abstractions;

namespace MyNewProjectName.Grammars.Business;

[ExcludeFromCodeCoverage]
internal sealed class SqlServerSyntaxFormatter : IDatabaseSpecificSyntaxFormatter
{ 
    public int ParameterStartIndex => 0;
    public string FormatIdentifier(string identifier) => $"[{identifier}]";
    public string GetParameterName(int index) => $"@p{index}";
    public string GetBindParameterName(int index) => GetParameterName(index);
}
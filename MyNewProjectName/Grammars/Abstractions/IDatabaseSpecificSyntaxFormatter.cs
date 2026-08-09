namespace MyNewProjectName.Grammars.Abstractions;

public interface IDatabaseSpecificSyntaxFormatter
{
    string FormatIdentifier(string identifier);
    string GetParameterName(int index); 
    int ParameterStartIndex { get; }
}
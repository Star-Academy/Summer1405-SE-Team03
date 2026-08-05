namespace MyNewProjectName.Grammars.Abstractions;

public interface ISqlGrammarBase
{
    string FormatIdentifier(string identifier);
    string GetParameterName(int index);
}
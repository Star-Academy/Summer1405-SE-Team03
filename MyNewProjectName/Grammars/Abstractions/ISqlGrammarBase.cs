namespace MyNewProjectName.Grammars;

public interface ISqlGrammarBase
{
    string FormatIdentifier(string identifier);
    string GetParameterName(int index);
}
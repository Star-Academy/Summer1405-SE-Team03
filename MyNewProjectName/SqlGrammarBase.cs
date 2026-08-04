namespace MyNewProjectName;

public interface SqlGrammarBase
{
    string FormatIdentifier(string identifier);
    string GetParameterName(int index);
}
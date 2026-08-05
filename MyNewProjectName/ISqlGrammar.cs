namespace MyNewProjectName;

public interface ISqlGrammar : SqlGrammarBase
{
    int ParameterStartIndex { get; }
}
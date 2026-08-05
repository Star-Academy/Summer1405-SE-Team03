namespace MyNewProjectName.Grammars;

public interface ISqlGrammar : ISqlGrammarBase
{
    int ParameterStartIndex { get; }
}
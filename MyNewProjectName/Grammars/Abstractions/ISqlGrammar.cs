namespace MyNewProjectName.Grammars.Abstractions;

public interface ISqlGrammar : ISqlGrammarBase
{
    int ParameterStartIndex { get; }
}
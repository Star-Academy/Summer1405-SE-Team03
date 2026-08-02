namespace MyNewProjectName;

public interface ISqlDialect
{
    int ParameterStartIndex { get; }
    string Quote(string identifier);
    string GetParameterName(int index);
}
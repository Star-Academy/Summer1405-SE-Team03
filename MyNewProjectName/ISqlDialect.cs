namespace MyNewProjectName;

public interface ISqlDialect
{
    string Quote(string identifier);
    string GetParameterName(int index);
}
namespace MyNewProjectName;

public abstract class SqlDialectBase : ISqlDialect
{
    public int ParameterStartIndex { get; }

    protected SqlDialectBase(int parameterStartIndex)
    {
        ParameterStartIndex = parameterStartIndex;
    }

    public abstract string Quote(string identifier);
    public abstract string GetParameterName(int index);
}
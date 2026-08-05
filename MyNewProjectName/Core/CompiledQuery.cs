namespace MyNewProjectName.Core;

public class CompiledQuery(string sql, IList<object> bindings)
{
    public string Sql { get; } = sql;
    public IList<object> Bindings { get; } = bindings;
}
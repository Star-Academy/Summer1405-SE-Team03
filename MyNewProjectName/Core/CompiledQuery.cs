namespace MyNewProjectName.Core;

public class CompiledQuery(string sqlQuery, IList<object> bindings)
{
    public string SqlQuery { get; } = sqlQuery;
    public IList<object> Bindings { get; } = bindings;
}
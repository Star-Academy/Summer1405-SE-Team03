namespace MyNewProjectName.Core;

public class CompiledQuery(string sqlQuery)
{ 
    public string SqlQuery { get; } = sqlQuery;
}
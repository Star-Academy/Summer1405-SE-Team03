namespace MyNewProjectName;
public class Query
{
    public string TableName { get; private set; }
    public List<string> Columns { get; set; } = new();
    public List<WhereCondition> WhereConditions { get; set; } = new();
    public Query From(string tableName)
    {
        TableName = tableName;
        return this;
    }
    public Query Where(string columnName, object value, string op = "=")
    {
        WhereConditions.Add(new WhereCondition(columnName, value, op));
        return this;
    }
    public Query Select(params string[] columns)
    {
        Columns.AddRange(columns);
        return this;
    }
}
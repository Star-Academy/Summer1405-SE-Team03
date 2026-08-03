namespace MyNewProjectName;

public class WhereClause
{
    public WhereClause(string columnName, object value)
    {
        ColumnName = columnName;
        Value = value;
    }

    public string ColumnName { get; set; }
    public object Value { get; set; }
}
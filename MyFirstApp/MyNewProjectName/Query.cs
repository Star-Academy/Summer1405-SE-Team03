namespace MyNewProjectName;
public class Query
{
    public string? _tableName { get; set; }
    public List<string> _columns{ get; set; } = new();
    public Dictionary<string, Object> _columnName_value{ get; set; } = new();
    public Query From(string tableName)
    {
        _tableName = tableName;
        return this;
    }

    public Query Where(string columnName, object value)
    {
        _columnName_value.Add(columnName,value);
        return this;
    }

    public Query Select(params string[] columns)
    {
        _columns.AddRange(columns);
        return this;
    }
    
    // public override string ToString()
    // {
    //     var cols = _columns.Count > 0 ? string.Join(", ", _columns) : "*";
    //     var query = $"SELECT {cols} FROM {_tableName}";
    //
    //     if (_columnName_value.Count > 0)
    //     {
    //         var conditions = _columnName_value.Select(kv =>
    //         {
    //             var val = kv.Value is string ? $"'{kv.Value}'" : kv.Value;
    //             return $"{kv.Key} = {val}";
    //         });
    //
    //         query += " WHERE " + string.Join(" AND ", conditions);
    //     }
    //
    //     return query;
    // }
}
    
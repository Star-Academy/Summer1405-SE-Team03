namespace MyNewProjectName;

public record WhereCondition(string ColumnName, object Value, string Operator = "=");
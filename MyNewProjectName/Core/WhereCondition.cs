namespace MyNewProjectName.Core;

public record WhereCondition(string ColumnName, object Value, string Operator = "=");
using System.Text;

namespace MyNewProjectName;

public class PostgresCompiler
{
    public (string sql , string binding) Compile(Query query)
    {
        StringBuilder message = new StringBuilder();
        StringBuilder paramsBinding = new();
        paramsBinding.Append("Bindings: ");
        message.Append("SELECT ");
        foreach (var column in query._columns)
        {
            message.Append('"').Append(column).Append('"').Append(", ");
        }
        if (message.Length >= 2)
        {
            message.Remove(message.Length - 2, 2);
        }
        message.Append(' ');
        message.Append("FROM ").Append('"').Append(query._tableName).Append('"').Append(' ');
        message.Append("WHERE ");
        int index = 1;
        paramsBinding.Append('[');
        foreach (var conditional_key in query._columnName_value)
        {
            message.Append('"').Append(conditional_key.Key).Append('"').Append(" = $").Append(index).Append(" AND ");
            paramsBinding.Append(conditional_key.Value).Append(", ");
            index++;
        }
        if (paramsBinding.Length >= 2)
        {
            paramsBinding.Remove(paramsBinding.Length - 2, 2);
        }
        paramsBinding.Append(']');
        if (message.Length >= 5)
        {
            message.Remove(message.Length - 5, 5);
        }
        return (message.ToString(), paramsBinding.ToString());
    }
}
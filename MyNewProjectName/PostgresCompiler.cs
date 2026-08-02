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
        foreach (var column in query.Columns)
        {
            message.Append('"').Append(column).Append('"').Append(", ");
        }
        if (message.Length >= 2)
        {
            message.Remove(message.Length - 2, 2);
        }
        message.Append(' ');
        message.Append("FROM ").Append('"').Append(query.TableName).Append('"').Append(' ');
        message.Append("WHERE ");
        int index = 1;
        paramsBinding.Append('[');
        foreach (var conditionalKey in query.ColumnNameValue)
        {
            message.Append('"').Append(conditionalKey.Key).Append('"').Append(" = $").Append(index).Append(" AND ");
            paramsBinding.Append(conditionalKey.Value).Append(", ");
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
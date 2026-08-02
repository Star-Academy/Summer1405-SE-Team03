using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace MyNewProjectName;

public class SqlCompiler : ISqlCompiler
{
    private readonly ISqlDialect _dialect;

    public SqlCompiler(ISqlDialect dialect)
    {
        _dialect = dialect;
    }

    public (string sql, string binding) Compile(Query query)
    {
        var sql = new StringBuilder();
        
        sql.Append("SELECT ");
        
        var quotedColumns = query.Columns.Select(c => _dialect.Quote(c));
        sql.Append(string.Join(", ", quotedColumns));

        sql.Append(" FROM ").Append(_dialect.Quote(query.TableName!)).Append(" WHERE ");

        int index = _dialect.ParameterStartIndex;
        var conditions = new List<string>();
        var bindingValues = new List<string>();

        foreach (var condition in query.ColumnNameValue)
        {
            string paramName = _dialect.GetParameterName(index);
            conditions.Add($"{_dialect.Quote(condition.Key)} = {paramName}");
            bindingValues.Add(condition.Value.ToString()!);
            
            index++;
        }

        sql.Append(string.Join(" AND ", conditions));
        
        string bindings = $"Bindings: [{string.Join(", ", bindingValues)}]";

        return (sql.ToString(), bindings);
    }
}
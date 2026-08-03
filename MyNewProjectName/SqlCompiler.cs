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

    public ISqlCompiler.CompilationResult Compile(Query query)
    {
        var queryTextBuilder = new StringBuilder();
        
        BuildSelectClause(queryTextBuilder, query);
        BuildFromClause(queryTextBuilder, query);
        string bindings = BuildWhereClause(queryTextBuilder, query);

        return new ISqlCompiler.CompilationResult(queryTextBuilder.ToString(), bindings);
    }

    private void BuildSelectClause(StringBuilder sql, Query query)
    {
        sql.Append("SELECT ");
        var quotedColumns = query.Columns.Select(c => _dialect.Quote(c));
        sql.Append(string.Join(", ", quotedColumns));
    }
    private void BuildFromClause(StringBuilder sql, Query query)
    {
        sql.Append(" FROM ").Append(_dialect.Quote(query.TableName!));
    }

    private string BuildWhereClause(StringBuilder sql, Query query)
    {
        if (!query.WhereClauses.Any()) return "Bindings: []";

        sql.Append(" WHERE ");
        
        var processed = ProcessWhereClauses(query.WhereClauses);

        sql.Append(string.Join(" AND ", processed.Conditions));
        return $"Bindings: [{string.Join(", ", processed.BindingValues)}]";
    }
    
    private (List<string> Conditions, List<string> BindingValues) ProcessWhereClauses(List<WhereClause> clauses)
    {
        var conditions = new List<string>();
        var bindingValues = new List<string>();
        int index = _dialect.ParameterStartIndex;

        foreach (var condition in clauses)
        {
            string paramName = _dialect.GetParameterName(index);
            conditions.Add($"{_dialect.Quote(condition.ColumnName)} = {paramName}");
            bindingValues.Add(condition.Value?.ToString() ?? DBNull.Value.ToString()!);
            index++;
        }

        return (conditions, bindingValues);
    }
}
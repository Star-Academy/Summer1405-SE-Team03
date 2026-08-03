using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace MyNewProjectName;

public class SqlCompiler : ISqlCompiler
{
    private readonly SqlDialectBase _dialect;

    public SqlCompiler(SqlDialectBase dialect)
    {
        _dialect = dialect;
    }

    public ISqlCompiler.CompilationResult Compile(Query query)
    {
        var queryTextBuilder = new StringBuilder();
        
        BuildSelectClause(queryTextBuilder, query);
        BuildFromClause(queryTextBuilder, query);
        var bindings = BuildWhereClause(queryTextBuilder, query);

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

    private List<object> BuildWhereClause(StringBuilder sql, Query query)
    {
        if (!query.WhereConditions.Any()) return new List<object>();

        sql.Append(" WHERE ");
        
        var processed = ProcessWhereClauses(query.WhereConditions);

        sql.Append(string.Join(" AND ", processed.Conditions));
        return processed.BindingValues; 
    }
    
    private (List<string> Conditions, List<object> BindingValues) ProcessWhereClauses(List<WhereCondition> clauses)
    {
        var conditions = new List<string>();
        var bindingValues = new List<object>();
        
        int index = _dialect.ParameterStartIndex;

        foreach (var condition in clauses)
        {
            string paramName = _dialect.GetParameterName(index);
            conditions.Add($"{_dialect.Quote(condition.ColumnName)} = {paramName}");
            
            bindingValues.Add(condition.Value ?? DBNull.Value);
            index++;
        }

        return (conditions, bindingValues);
    }
}
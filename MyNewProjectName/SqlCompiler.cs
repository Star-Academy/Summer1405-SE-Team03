using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace MyNewProjectName;

public class SqlCompiler : ISqlCompiler
{
    private readonly SqlGrammar _dialect;

    public SqlCompiler(SqlGrammar dialect)
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

    private void BuildSelectClause(StringBuilder queryBuilder, Query query)
    {
        queryBuilder.Append("SELECT ");
        var quotedColumns = query.Columns.Select(c => _dialect.FormatIdentifier(c));
        queryBuilder.Append(string.Join(", ", quotedColumns));
    }
    
    private void BuildFromClause(StringBuilder queryBuilder, Query query)
    {
        if (string.IsNullOrWhiteSpace(query.TableName))
        {
            throw new InvalidOperationException("Table name cannot be null or empty.");
        }
        queryBuilder.Append(" FROM ").Append(_dialect.FormatIdentifier(query.TableName!));
    }

    private List<object> BuildWhereClause(StringBuilder queryBuilder, Query query)
    {
        if (!query.WhereConditions.Any()) return new List<object>();

        queryBuilder.Append(" WHERE ");
        
        var processed = ProcessWhereClauses(query.WhereConditions);

        queryBuilder.Append(string.Join(" AND ", processed.Conditions));
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
            conditions.Add($"{_dialect.FormatIdentifier(condition.ColumnName)} = {paramName}");
            
            bindingValues.Add(condition.Value ?? DBNull.Value);
            index++;
        }

        return (conditions, bindingValues);
    }
}
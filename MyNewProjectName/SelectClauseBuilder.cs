using System;
using System.Linq;
using System.Text;

namespace MyNewProjectName;

public class SelectClauseBuilder(ISqlGrammar sqlGrammar) : ISelectClauseBuilder
{
    private readonly ISqlGrammar _sqlGrammar = sqlGrammar ?? throw new ArgumentNullException(nameof(sqlGrammar));

    public void Build(StringBuilder queryBuilder, Query query)
    {
        queryBuilder.Append("SELECT ");
        
        var quotedColumns = query.Columns.Select(c => _sqlGrammar.FormatIdentifier(c));
        queryBuilder.Append(string.Join(", ", quotedColumns));
    }
}
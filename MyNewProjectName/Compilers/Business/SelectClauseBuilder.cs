using System.Text;
using MyNewProjectName.Core;
using MyNewProjectName.Grammars;

namespace MyNewProjectName.Compilers.Builders;

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
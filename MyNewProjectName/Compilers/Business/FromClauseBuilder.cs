using System.Text;
using MyNewProjectName.Core;
using MyNewProjectName.Grammars;

namespace MyNewProjectName.Compilers.Builders;

public class FromClauseBuilder(ISqlGrammar sqlGrammar) : IFromClauseBuilder
{
    private readonly ISqlGrammar _sqlGrammar = sqlGrammar ?? throw new ArgumentNullException(nameof(sqlGrammar));

    public void Build(StringBuilder queryBuilder, Query query)
    {
        if (string.IsNullOrWhiteSpace(query.TableName))
        {
            throw new InvalidOperationException("Table name cannot be null or empty.");
        }
        
        queryBuilder.Append(" FROM ").Append(_sqlGrammar.FormatIdentifier(query.TableName!));
    }
}
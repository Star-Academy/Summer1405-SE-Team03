using System;
using System.Text;
using MyNewProjectName.Compilers.Abstractions;
using MyNewProjectName.Core;
using MyNewProjectName.Grammars.Abstractions;

namespace MyNewProjectName.Compilers.Business;

internal sealed class FromClauseBuilder : IFromClauseBuilder
{
    private readonly IDatabaseSpecificSyntaxFormatter _databaseSpecificSyntaxFormatter;

    public FromClauseBuilder(IDatabaseSpecificSyntaxFormatter databaseSpecificSyntaxFormatter)
    {
        _databaseSpecificSyntaxFormatter = databaseSpecificSyntaxFormatter ?? throw new ArgumentNullException(nameof(databaseSpecificSyntaxFormatter));
    }

    public void Build(StringBuilder queryBuilder, Query query)
    {
        if (string.IsNullOrWhiteSpace(query.TableName))
        {
            throw new InvalidOperationException("Table name cannot be null or empty.");
        }

        queryBuilder.Append(" FROM ").Append(_databaseSpecificSyntaxFormatter.FormatIdentifier(query.TableName!));
    }
}
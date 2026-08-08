using System;
using System.Linq;
using System.Text;
using MyNewProjectName.Compilers.Abstractions;
using MyNewProjectName.Core;
using MyNewProjectName.Grammars.Abstractions;

namespace MyNewProjectName.Compilers.Business;

internal sealed class SelectClauseBuilder : ISelectClauseBuilder
{
    private readonly IDatabaseSpecificSyntaxFormatter _databaseSpecificSyntaxFormatter;

    public SelectClauseBuilder(IDatabaseSpecificSyntaxFormatter databaseSpecificSyntaxFormatter)
    {
        _databaseSpecificSyntaxFormatter = databaseSpecificSyntaxFormatter ?? throw new ArgumentNullException(nameof(databaseSpecificSyntaxFormatter));
    }

    public StringBuilder Build(StringBuilder queryBuilder, Query query)
    {
        queryBuilder.Append("SELECT ");
        
        var quotedColumns = query.Columns.Select(c => _databaseSpecificSyntaxFormatter.FormatIdentifier(c));
        queryBuilder.Append(string.Join(", ", quotedColumns));

        return queryBuilder;
    }
}
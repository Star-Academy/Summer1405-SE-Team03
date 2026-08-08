using System;
using System.Linq;
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

    public string Build(Query query)
    {
        var quotedColumns = query.Columns.Select(c => _databaseSpecificSyntaxFormatter.FormatIdentifier(c));
        return "SELECT " + string.Join(", ", quotedColumns);
    }
}
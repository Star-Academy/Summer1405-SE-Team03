using System;
using MyNewProjectName.Compilers.Abstractions;
using MyNewProjectName.Core;

namespace MyNewProjectName.Compilers.Business;

internal sealed class SqlQueryCompiler : ISqlQueryCompiler
{
    private readonly ISelectClauseBuilder _selectClauseBuilder;
    private readonly IFromClauseBuilder _fromClauseBuilder;
    private readonly IWhereClauseBuilder _whereClauseBuilder;

    public SqlQueryCompiler(
        ISelectClauseBuilder selectClauseBuilder,
        IFromClauseBuilder fromClauseBuilder,
        IWhereClauseBuilder whereClauseBuilder)
    {
        _selectClauseBuilder = selectClauseBuilder ?? throw new ArgumentNullException(nameof(selectClauseBuilder));
        _fromClauseBuilder = fromClauseBuilder ?? throw new ArgumentNullException(nameof(fromClauseBuilder));
        _whereClauseBuilder = whereClauseBuilder ?? throw new ArgumentNullException(nameof(whereClauseBuilder));
    }

    public CompiledQuery Compile(Query query)
    {
        var selectClause = _selectClauseBuilder.Build(query);
        var fromClause = _fromClauseBuilder.Build(query);
        var whereClause = _whereClauseBuilder.Build(query);

        var fullSql = $"{selectClause}{fromClause}{whereClause.SqlText}";
        return new CompiledQuery(fullSql);
    }
}
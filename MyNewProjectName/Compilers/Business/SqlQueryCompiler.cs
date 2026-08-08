using System;
using System.Text;
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
        var queryTextBuilder = new StringBuilder();
        
        _selectClauseBuilder.Build(queryTextBuilder, query);
        _fromClauseBuilder.Build(queryTextBuilder, query);
        var bindings = _whereClauseBuilder.Build(queryTextBuilder, query);

        return new CompiledQuery(queryTextBuilder.ToString(), bindings);
    }
}
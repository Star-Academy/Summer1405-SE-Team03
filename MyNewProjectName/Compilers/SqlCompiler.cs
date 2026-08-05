using System;
using System.Text;

namespace MyNewProjectName;

public class SqlCompiler(
    ISelectClauseBuilder selectClauseBuilder,
    IFromClauseBuilder fromClauseBuilder,
    IWhereClauseBuilder whereClauseBuilder) : ISqlCompiler
{
    private readonly ISelectClauseBuilder _selectClauseBuilder = selectClauseBuilder ?? throw new ArgumentNullException(nameof(selectClauseBuilder));
    private readonly IFromClauseBuilder _fromClauseBuilder = fromClauseBuilder ?? throw new ArgumentNullException(nameof(fromClauseBuilder));
    private readonly IWhereClauseBuilder _whereClauseBuilder = whereClauseBuilder ?? throw new ArgumentNullException(nameof(whereClauseBuilder));

    public CompiledQuery Compile(Query query)
    {
        var queryTextBuilder = new StringBuilder();
        _selectClauseBuilder.Build(queryTextBuilder, query);
        _fromClauseBuilder.Build(queryTextBuilder, query);
        var bindings = _whereClauseBuilder.Build(queryTextBuilder, query);
        return new CompiledQuery(queryTextBuilder.ToString(), bindings);
    }
}
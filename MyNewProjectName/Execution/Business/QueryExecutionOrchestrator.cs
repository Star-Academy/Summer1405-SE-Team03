using System;
using MyNewProjectName.Compilers.Abstractions;
using MyNewProjectName.Core;
using MyNewProjectName.Execution.Abstractions;

namespace MyNewProjectName.Execution.Business;

internal sealed class QueryExecutionOrchestrator : IQueryExecutor
{
    private readonly ISqlQueryCompiler _compiler;
    private readonly ICompiledQueryRunner _dbRunner;

    public QueryExecutionOrchestrator(
        ISqlQueryCompiler compiler,
        ICompiledQueryRunner dbRunner)
    {
        _compiler = compiler ?? throw new ArgumentNullException(nameof(compiler));
        _dbRunner = dbRunner ?? throw new ArgumentNullException(nameof(dbRunner));
    }

    public void ExecuteQuery(Query query)
    {
        var compiledResult = _compiler.Compile(query);
        _dbRunner.QueryRunner(compiledResult, query);
    }
}
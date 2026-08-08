using MyNewProjectName.Core;

namespace MyNewProjectName.Execution.Abstractions;

public interface ICompiledQueryRunner
{
    void QueryRunner(CompiledQuery compiledQuery, Query originalQuery);
}
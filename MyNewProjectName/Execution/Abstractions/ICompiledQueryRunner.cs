using MyNewProjectName.Core;

namespace MyNewProjectName.Execution.Abstractions;

public interface ICompiledQueryRunner
{
    void Run(CompiledQuery compiledQuery, Query originalQuery);
}
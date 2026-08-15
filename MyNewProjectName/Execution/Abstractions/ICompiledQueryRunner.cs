using System.Data;
using MyNewProjectName.Core;

namespace MyNewProjectName.Execution.Abstractions;

public interface ICompiledQueryRunner
{
    IDataReader QueryRunner(CompiledQuery compiledQuery, Query originalQuery);
}
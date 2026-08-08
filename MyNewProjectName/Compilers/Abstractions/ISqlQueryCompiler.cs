using MyNewProjectName.Core;

namespace MyNewProjectName.Compilers.Abstractions;

public interface ISqlQueryCompiler
{
    CompiledQuery Compile(Query query);
}
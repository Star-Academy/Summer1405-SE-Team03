using MyNewProjectName.Core;

namespace MyNewProjectName.Compilers.Abstractions;

public interface ISqlCompiler
{
    CompiledQuery Compile(Query query);
}
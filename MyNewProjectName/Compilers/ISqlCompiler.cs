using MyNewProjectName.Core;

namespace MyNewProjectName.Compilers;

public interface ISqlCompiler
{
    CompiledQuery Compile(Query query);
}
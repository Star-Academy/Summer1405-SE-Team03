namespace MyNewProjectName;

public interface ISqlCompiler
{
    CompiledQuery Compile(Query query);
}
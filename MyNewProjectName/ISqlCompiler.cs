namespace MyNewProjectName;

public interface ISqlCompiler
{
    public record CompilationResult(string Sql, string Binding);
    CompilationResult Compile(Query query);
}
namespace MyNewProjectName;

public interface ISqlCompiler
{
    public record CompilationResult(string Sql, List<object> Binding);
    CompilationResult Compile(Query query);
}
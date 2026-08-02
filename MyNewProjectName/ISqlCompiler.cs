namespace MyNewProjectName;

public interface ISqlCompiler
{
    (string sql, string binding) Compile(Query query);
}
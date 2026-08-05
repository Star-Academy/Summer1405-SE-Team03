namespace MyNewProjectName.Execution;

public record DatabaseOptions(string ConnectionString, string DatabaseName = "Database");
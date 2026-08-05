namespace MyNewProjectName.Execution.Business;

public record DatabaseOptions(string ConnectionString, string DatabaseName = "Database");
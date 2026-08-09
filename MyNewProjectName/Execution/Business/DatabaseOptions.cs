using System.Diagnostics.CodeAnalysis;

namespace MyNewProjectName.Execution.Business;

[ExcludeFromCodeCoverage]
public record DatabaseOptions(string ConnectionString, string DatabaseName);
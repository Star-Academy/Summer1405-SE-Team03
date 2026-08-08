namespace MyNewProjectName.Compilers.Business;

public record ProcessedWhereConditions(
    IList<string> Conditions, 
    IList<object> BindingValues
);
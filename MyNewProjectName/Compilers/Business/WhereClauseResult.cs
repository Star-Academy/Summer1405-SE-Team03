using System.Collections.Generic;

namespace MyNewProjectName.Compilers.Business;

public record WhereClauseResult(
    string SqlText, 
    IList<object> BindingValues
);
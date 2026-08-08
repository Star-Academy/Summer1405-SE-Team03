using System.Collections.Generic;
using MyNewProjectName.Core;
using MyNewProjectName.Execution.Business;

namespace MyNewProjectName.Execution.Abstractions;

public interface IQueryParameterBinder
{
    IReadOnlyList<QueryParameter> BindParameters(Query query);
}
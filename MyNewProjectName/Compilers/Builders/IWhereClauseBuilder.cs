using System.Collections.Generic;
using System.Text;

namespace MyNewProjectName;

public interface IWhereClauseBuilder
{
    IList<object> Build(StringBuilder queryBuilder, Query query);
}
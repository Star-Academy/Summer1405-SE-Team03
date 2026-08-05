using System.Text;
using MyNewProjectName.Core;

namespace MyNewProjectName.Compilers.Builders;

public interface IWhereClauseBuilder
{
    IList<object> Build(StringBuilder queryBuilder, Query query);
}
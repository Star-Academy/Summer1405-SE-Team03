using System.Text;
using MyNewProjectName.Core;

namespace MyNewProjectName.Compilers.Abstractions;

public interface IWhereClauseBuilder
{
    IList<object> Build(StringBuilder queryBuilder, Query query);
}
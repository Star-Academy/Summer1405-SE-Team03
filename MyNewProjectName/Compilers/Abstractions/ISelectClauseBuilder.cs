using System.Text;
using MyNewProjectName.Core;

namespace MyNewProjectName.Compilers.Abstractions;

public interface ISelectClauseBuilder
{
    StringBuilder Build(StringBuilder queryBuilder, Query query);
}
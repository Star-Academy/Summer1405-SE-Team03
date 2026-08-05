using System.Text;
using MyNewProjectName.Core;

namespace MyNewProjectName.Compilers.Builders;

public interface IFromClauseBuilder
{
    void Build(StringBuilder queryBuilder, Query query);
}
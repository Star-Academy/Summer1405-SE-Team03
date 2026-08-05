using System.Text;
using MyNewProjectName.Core;

namespace MyNewProjectName.Compilers.Builders;

public interface ISelectClauseBuilder
{
    void Build(StringBuilder queryBuilder, Query query);
}
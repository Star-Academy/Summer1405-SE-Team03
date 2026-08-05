using System.Text;
using MyNewProjectName.Core;

namespace MyNewProjectName.Compilers.Abstractions;

public interface IFromClauseBuilder
{
    void Build(StringBuilder queryBuilder, Query query);
}
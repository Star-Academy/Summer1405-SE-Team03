using System.Text;
using MyNewProjectName.Core;

namespace MyNewProjectName.Compilers.Abstractions;

public interface ISelectClauseBuilder
{
    void Build(StringBuilder queryBuilder, Query query);
}
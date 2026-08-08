using MyNewProjectName.Compilers.Business;
using MyNewProjectName.Core;

namespace MyNewProjectName.Compilers.Abstractions;

public interface IWhereClauseBuilder
{
    WhereClauseResult Build(Query query);
}
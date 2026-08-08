using MyNewProjectName.Core;

namespace MyNewProjectName.Compilers.Abstractions;

public interface ISelectClauseBuilder
{
    string Build(Query query);
}
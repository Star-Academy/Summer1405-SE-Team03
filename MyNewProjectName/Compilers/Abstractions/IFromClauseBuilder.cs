using System.Text;
using MyNewProjectName.Core;

namespace MyNewProjectName.Compilers.Abstractions;

public interface IFromClauseBuilder
{
    string Build(Query query);
}
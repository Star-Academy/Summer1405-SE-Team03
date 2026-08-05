using System.Text;

namespace MyNewProjectName;

public interface IFromClauseBuilder
{
    void Build(StringBuilder queryBuilder, Query query);
}
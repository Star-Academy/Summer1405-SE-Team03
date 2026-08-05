using System.Text;

namespace MyNewProjectName;

public interface ISelectClauseBuilder
{
    void Build(StringBuilder queryBuilder, Query query);
}
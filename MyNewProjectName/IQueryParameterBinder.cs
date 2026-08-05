using System.Data;

namespace MyNewProjectName;

public interface IQueryParameterBinder
{
    void AddParameters(IDbCommand dbCommand, Query query);
}
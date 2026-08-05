using System.Data;

namespace MyNewProjectName;

public interface IDbCommandFactory
{
    IDbCommand CreateCommand(string sqlText, IDbConnection dbConnection);
}
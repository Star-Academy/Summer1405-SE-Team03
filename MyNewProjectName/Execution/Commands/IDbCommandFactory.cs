using System.Data;

namespace MyNewProjectName.Execution.Commands;

public interface IDbCommandFactory
{
    IDbCommand CreateCommand(string sqlText, IDbConnection dbConnection);
}
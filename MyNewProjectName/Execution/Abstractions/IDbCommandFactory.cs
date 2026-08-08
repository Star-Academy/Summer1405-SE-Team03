using System.Data;

namespace MyNewProjectName.Execution.Abstractions;

public interface IDbCommandFactory
{
    IDbCommand CreateCommand(string sqlText, IDbConnection dbConnection);
}